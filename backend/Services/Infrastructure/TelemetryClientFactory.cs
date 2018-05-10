using backend.Interfaces.Database;
using backend.Interfaces.Infrastructure;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public class TelemetryClientFactory : ITelemetryClientFactory
    {
        private const string AIKEY = "ai_key";
        private readonly ISecretStore secretStore;

        public TelemetryClientFactory(ISecretStore secretStore)
        {
            this.secretStore = secretStore;
        }

        public TelemetryClient Create()
        {
            return new TelemetryClient(new TelemetryConfiguration()
            {
                InstrumentationKey = GetInstrumentationKey(),
            });
        }

        public string GetInstrumentationKey()
        {
            return secretStore.Get(AIKEY);
        }
    }
}
