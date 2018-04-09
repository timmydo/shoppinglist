using backend.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public class DependencyTracker : IDependencyTracker
    {
        public async Task<T> TrackAsync<T>(string area, string method, Func<IDependencyInvocation, Task<T>> action)
        {
            var invocation = new DependencyInvocation();
            try
            {
                var res = await action(invocation);
                return res;
            }
            catch (Exception)
            {
                // todo log it
                throw;
            }
        }

        private class DependencyInvocation : IDependencyInvocation
        {
            public DependencyInvocation()
            {
            }
        }
    }
}
