using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Services.Database;
using backend.Services.Infrastructure;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            var config = Startup.BuildConfiguration();
            var tcf = new TelemetryClientFactory(new SecretStore(null));
            return builder
                .UseApplicationInsights(tcf.GetInstrumentationKey())
                .UseStartup<Startup>()
                .Build();
        }
    }
}
