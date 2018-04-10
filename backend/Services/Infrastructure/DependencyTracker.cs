using backend.Interfaces.Infrastructure;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public class DependencyTracker : IDependencyTracker
    {
        private readonly TelemetryClient client;

        public DependencyTracker()
        {
            this.client = new TelemetryClient();
        }

        public async Task<T> TrackAsync<T>(string area, string method, Func<IDependencyInvocation, Task<T>> action)
        {
            var invocation = new DependencyInvocation();
            var watch = new Stopwatch();
            watch.Start();
            var start = DateTimeOffset.UtcNow;
            try
            {
                var res = await action(invocation);
                watch.Stop();
                client.TrackDependency(area, method, start, watch.Elapsed, true);
                return res;
            }
            catch (Exception e)
            {
                watch.Stop();
                client.TrackDependency(area, method, start, watch.Elapsed, false);
                client.TrackException(e);
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
