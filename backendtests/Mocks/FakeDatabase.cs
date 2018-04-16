using backend.Interfaces.Database;
using backend.Models.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace backendtests.Mocks
{
    public class FakeDatabase : IDatabase
    {
        public IDictionary<string, string> Database { get; } = new Dictionary<string, string>();

        public Task Create<T>(T obj) where T : IDocumentObject
        {
            Database.Add(obj.Id, JsonConvert.SerializeObject(obj));
            return Task.CompletedTask;
        }

        public Task<T> Read<T>(string id) where T : IDocumentObject
        {
            if (Database.TryGetValue(id, out var strVal))
            {
                return Task.FromResult<T>(JsonConvert.DeserializeObject<T>(strVal));
            }
            else
            {
                return Task.FromResult<T>(default(T));
            }
        }

        public Task Write<T>(T obj) where T : IDocumentObject
        {
            Database[obj.Id] = JsonConvert.SerializeObject(obj);
            return Task.CompletedTask;
        }
    }
}
