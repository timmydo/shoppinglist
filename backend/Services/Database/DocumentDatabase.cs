using backend.Interfaces.Database;
using backend.Interfaces.Infrastructure;
using backend.Models.Config;
using backend.Models.Documents;
using backend.Models.Exceptions;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace backend.Services.Database
{
    public class DocumentDatabase : IDatabase
    {
        private readonly IDocumentSerializer documentSerializer;
        private readonly DatabaseSettings options;
        private readonly DocumentClient client;
        private readonly IMetricTracker ruCounter;

        public DocumentDatabase(IDocumentSerializer documentSerializer, ISecretStore secretStore, IOptions<DatabaseSettings> options, IMetricTrackerFactory metricTrackerFactory)
        {
            this.documentSerializer = documentSerializer;
            this.options = options.Value;
            var uri = new Uri(this.options.ServiceEndpoint);
            this.ruCounter = metricTrackerFactory.Create($"RU_{uri.Host}");
            var authKey = secretStore.Get(this.options.AuthKey);
            var cp = new ConnectionPolicy()
            {
                RequestTimeout = TimeSpan.FromSeconds(5),
                RetryOptions = new RetryOptions()
                {
                    MaxRetryAttemptsOnThrottledRequests = 1,
                    MaxRetryWaitTimeInSeconds = 2,
                },
            };

            this.client = new DocumentClient(uri, authKey, cp);
        }

        public async Task Create<T>(T obj) where T : IDocumentObject
        {
            var uri = UriFactory.CreateDocumentCollectionUri(options.Database, options.Collection);
            var item = documentSerializer.Serialize(obj);
            try
            {
                var res = await client.CreateDocumentAsync(uri, item);
                ruCounter.Track(res.RequestCharge);
                obj.Etag = res.Resource.ETag;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode != HttpStatusCode.Conflict)
                {
                    throw;
                }
            }
        }

        public async Task<T> Read<T>(string id) where T : IDocumentObject
        {
            try
            {
                var ro = new RequestOptions
                {
                    PartitionKey = new PartitionKey(id),
                };

                var uri = UriFactory.CreateDocumentUri(options.Database, options.Collection, id);

                var document = await client.ReadDocumentAsync<DatabaseObject>(uri, ro);
                ruCounter.Track(document.RequestCharge);

                var obj = document.Document;
                return documentSerializer.Deserialize<T>(obj);
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(T);
                }
                else
                {
                    throw new BackendException(e.Message, e);
                }
            }
        }

        public async Task Write<T>(T obj) where T : IDocumentObject
        {

            var uri = UriFactory.CreateDocumentUri(options.Database, options.Collection, obj.Id);
            var ro = new RequestOptions
            {
                AccessCondition = new AccessCondition()
                {
                    Condition = obj.Etag,
                    Type = AccessConditionType.IfMatch,
                },
            };

            try
            {
                var serialized = documentSerializer.Serialize(obj);
                var res = await client.ReplaceDocumentAsync(uri, serialized, ro);
                ruCounter.Track(res.RequestCharge);
                obj.Etag = res.Resource.ETag;
            }
            catch (DocumentClientException de)
            {
                if (de.StatusCode == HttpStatusCode.PreconditionFailed)
                {
                    throw new EtagMismatchException("ETagMismatch", de);
                }
                else if (de.StatusCode.HasValue && (int)de.StatusCode.Value == 429)
                {
                    throw new ThrottledException(nameof(Write));
                }
                else if (de.StatusCode.HasValue && (int)de.StatusCode.Value == 408)
                {
                    throw new BackendTimeoutException(nameof(Write));
                }
                else
                {
                    throw new BackendException(de.Message, de);
                }
            }
        }
    }
}
