using backend.Interfaces.Infrastructure;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Infrastructure
{
    public class MetricTrackerFactory : IMetricTrackerFactory
    {
        private readonly TelemetryClient client;

        public MetricTrackerFactory()
        {
            this.client = new TelemetryClient();
        }

        public IMetricTracker Create(string name)
        {
            return new MetricTracker(client, name);
        }

        private class MetricTracker : IMetricTracker
        {
            private readonly TelemetryClient client;
            private readonly string name;

            public MetricTracker(TelemetryClient client, string name)
            {
                this.client = client;
                this.name = name;
            }

            public void Track(double amt)
            {
                var telemetry = new MetricTelemetry(name, amt);
                client.TrackMetric(telemetry);
            }
        }
    }
}
