using backend.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Bot
{
    public class BotCredentialProvider : Microsoft.Bot.Connector.Authentication.ICredentialProvider
    {
        private readonly ISecretStore secrets;
        private readonly bool isDevelopment;

        public BotCredentialProvider(ISecretStore secrets, bool isDevelopment)
        {
            this.secrets = secrets;
            this.isDevelopment = isDevelopment;
        }

        public Task<string> GetAppPasswordAsync(string appId)
        {
            return Task.FromResult(secrets.Get($"app_pw_{appId}"));
        }

        public Task<bool> IsAuthenticationDisabledAsync()
        {
            return Task.FromResult(isDevelopment);
        }

        public Task<bool> IsValidAppIdAsync(string appId)
        {
            return Task.FromResult(!string.IsNullOrEmpty(secrets.Get($"app_pw_{appId}")));
        }
    }
}
