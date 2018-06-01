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

        public BotCredentialProvider(ISecretStore secrets)
        {
            this.secrets = secrets;
        }

        public Task<string> GetAppPasswordAsync(string appId)
        {
            return Task.FromResult(secrets.Get($"app_pw_{appId}"));
        }

        public Task<bool> IsAuthenticationDisabledAsync()
        {
            return Task.FromResult(false);
        }

        public Task<bool> IsValidAppIdAsync(string appId)
        {
            return Task.FromResult(!string.IsNullOrEmpty(secrets.Get($"app_pw_{appId}")));
        }
    }
}
