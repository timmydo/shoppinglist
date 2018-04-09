using backend.Interfaces.Api;
using backend.Interfaces.Database;
using backend.Interfaces.Infrastructure;
using backend.Services.Api;
using backend.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void Register(IServiceContainer container)
        {
            container.AddSingleton<IBinaryEncoder, BinaryEncoder>();
            container.AddSingleton<ICompressor, Compressor>();
            container.AddSingleton<IDatabase, DocumentDatabase>();
            container.AddSingleton<IDocumentSerializer, DocumentSerializer>();
            container.AddSingleton<ISecretStore, SecretStore>();
            container.AddSingleton<IDependencyTracker, DependencyTracker>();
            container.AddSingleton<IUserApi, UserApi>();
        }
    }
}
