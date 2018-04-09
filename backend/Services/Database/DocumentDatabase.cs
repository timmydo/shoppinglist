using backend.Interfaces.Database;
using backend.Models.Config;
using backend.Models.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Database
{
    public class DocumentDatabase : IDatabase
    {
        private readonly IDocumentSerializer documentSerializer;
        private readonly DatabaseSettings options;
        private readonly DocumentClient client;

        public DocumentDatabase(IDocumentSerializer documentSerializer, ISecretStore secretStore, IOptions<DatabaseSettings> options)
        {
            this.documentSerializer = documentSerializer;
            this.options = options.Value;
            var authKey = secretStore.Get(this.options.AuthKey);
            var cp = new ConnectionPolicy()
            {
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                RequestTimeout = TimeSpan.FromSeconds(5),
                RetryOptions = new RetryOptions()
                {
                    MaxRetryAttemptsOnThrottledRequests = 1,
                    MaxRetryWaitTimeInSeconds = 2,
                },
            };

            this.client = new DocumentClient(new Uri(this.options.ServiceEndpoint), authKey, cp);
        }

        public Task Create<T>(T obj) where T : IDocumentObject
        {
            throw new NotImplementedException();
        }

        public Task<T> Read<T>(string id) where T : IDocumentObject
        {
            throw new NotImplementedException();
        }

        public Task Write<T>(T obj) where T : IDocumentObject
        {
            throw new NotImplementedException();
        }
    }
}
