using backend.Interfaces.Api;
using backend.Interfaces.Auth;
using backend.Interfaces.Database;
using backend.Interfaces.Infrastructure;
using backend.Services.Api;
using backend.Services.Auth;
using backend.Services.Database;

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
            container.AddSingleton<IUserService, UserService>();
            container.AddSingleton<IMetricTrackerFactory, MetricTrackerFactory>();
        }
    }
}
