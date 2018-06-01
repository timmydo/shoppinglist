using System.IO;
using backend.Models;
using backend.Models.Config;
using backend.Services.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using backend.Services.Bot;
using backend.Services.Database;
using System.Collections.Generic;

namespace backend
{
    public class Startup
    {
        private readonly IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            Configuration = BuildConfiguration();
        }

        public static IConfigurationRoot BuildConfiguration()
        {
            var baseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            return new ConfigurationBuilder()
                .SetBasePath(baseDir)
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("MicrosoftAppId", ""),
                    new KeyValuePair<string, string>("MicrosoftAppPassword", ""),
                })
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

            services.Configure<DatabaseSettings>(Configuration.GetSection(Constants.ConfigurationSections.Database));
            services.Configure<SecretSettings>(Configuration.GetSection(Constants.ConfigurationSections.Secrets));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            ConfigureAuth(services);

            services.AddApplicationInsightsTelemetry();
            services.AddBot<ShoppingListBot>(options =>
            {
                options.CredentialProvider = new BotCredentialProvider(new SecretStore(null));
            });

            services.AddMvc();
        }

        private void ConfigureAuth(IServiceCollection services)
        {
            var tvp = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudiences = new string[] { "0cd9ecf8-f3ec-475e-8882-8292b40e7516" },
                ValidIssuers = new string[] { "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0" },
            };

            services.AddAuthentication(ao =>
            {
                ao.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwo =>
            {
                jwo.Authority = "https://login.microsoftonline.com/9188040d-6c67-4c5b-b112-36a304b66dad/v2.0";
                jwo.Audience = "0cd9ecf8-f3ec-475e-8882-8292b40e7516";
                jwo.TokenValidationParameters = tvp;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWhen(c => c.Request.Path.StartsWithSegments("/api/v1"),
                        subapp =>
                        {
                            subapp.UseAuthentication();
                        });

            // app.UseMiddleware<TimeoutMiddleware>(25000);

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseBotFramework();

            app.UseMvc();
        }
    }
}
