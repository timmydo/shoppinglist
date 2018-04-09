using backend.Interfaces.Database;
using backend.Models.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services.Database
{
    public class SecretStore : ISecretStore
    {
        private readonly SecretSettings options;

        public SecretStore(IOptions<SecretSettings> options)
        {
            this.options = options.Value;
        }

        public string Get(string key)
        {
            return File.ReadAllText(Path.Combine(this.options.BasePath, key + ".txt")).Trim();
        }
    }
}
