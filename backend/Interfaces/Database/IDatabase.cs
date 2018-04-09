using backend.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Database
{
    public interface IDatabase
    {
        Task<T> Read<T>(string id) where T : IDocumentObject;

        Task Create<T>(T obj) where T : IDocumentObject;

        Task Write<T>(T obj) where T : IDocumentObject;
    }
}
