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
            services.Configure<JwtSettings>(Configuration.GetSection("jwt"));
            services.Configure<ExternalTokenSettings>(Configuration.GetSection("sso"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var jwtConfig = new JwtSettings();
            Configuration.GetSection("jwt").Bind(jwtConfig);
            var sp = services.BuildServiceProvider();
            var sm = sp.GetRequiredService<Interfaces.Database.ISecretStore>();

            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = jwtConfig.ValidateAudience,
                ValidateIssuer = jwtConfig.ValidateIssuer,
                ValidateLifetime = jwtConfig.ValidateLifetime,
                ValidateIssuerSigningKey = jwtConfig.ValidateIssuerSigningKey,
                ValidAudiences = jwtConfig.ValidAudiences.Split(',').Select(iss => iss.Trim()).ToArray(),
                ValidIssuers = jwtConfig.ValidIssuers.Split(',').Select(iss => iss.Trim()).ToArray(),
                IssuerSigningKeys = jwtConfig.SymmetricSigningKeys.Split(',').Where(ssk => !string.IsNullOrWhiteSpace(ssk)).Select(ssk => new SymmetricSecurityKey(Convert.FromBase64String(sm.Get(ssk.Trim())))).ToArray(),
                RequireSignedTokens = true,
            };

            services.AddAuthentication(ao =>
            {
                ao.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwo =>
                {
                    jwo.Authority = jwtConfig.Authority;
                    jwo.Audience = jwtConfig.Audience;
                    jwo.TokenValidationParameters = tvp;
                });

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
