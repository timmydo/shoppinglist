using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public class ServiceContainer : Interfaces.Infrastructure.IServiceContainer
    {
        private readonly IServiceCollection serviceCollection;

        public ServiceContainer(IServiceCollection serviceCollection)
        {
            this.serviceCollection = serviceCollection;
        }

        public void AddSingleton<TInterface, TClass>()
        {
            serviceCollection.AddSingleton(typeof(TInterface), typeof(TClass));
        }
    }
}
