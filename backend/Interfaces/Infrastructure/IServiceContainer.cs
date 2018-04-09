using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Interfaces.Infrastructure
{
    public interface IServiceContainer
    {
        void AddSingleton<TInterface, TClass>();
    }
}
