using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Config
{
    public class ExternalTokenSettings
    {
        public string ValidIssuers { get; set; }

        public string ValidAudiences { get; set; }

        public bool ValidateIssuerSigningKey { get; set; }

        public bool ValidateIssuer { get; set; }

        public bool ValidateAudience { get; set; }

        public bool ValidateLifetime { get; set; }

        public string SymmetricSigningKeys { get; set; }

    }
}
