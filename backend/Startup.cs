using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using backend.Middleware;
using backend.Models.Config;
using backend.Services.Infrastructure;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace backend
{
    public class Startup
    {
        private readonly IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            Console.WriteLine("Current directory " + baseDir);
            Configuration = new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddIniFile("config.ini", optional: false)
                .AddEnvironmentVariables()
                .Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var c = new ServiceContainer(services);
            ServiceRegistration.Register(c);

            services.Configure<DatabaseSettings>(Configuration.GetSection("database"));
            services.Configure<SecretSettings>(Configuration.GetSection("secrets"));
            services.Configure<InternalTokenSettings>(Configuration.GetSection("internal"));
            services.Configure<ExternalTokenSettings>(Configuration.GetSection("external"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddApplicationInsightsTelemetry();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMiddleware<TimeoutMiddleware>(5000);
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }
    }
}
