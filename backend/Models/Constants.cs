using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public static class Constants
    {
        public static class ConfigurationSections
        {
            public const string Auth = "auth";

            public const string Secrets = "secrets";

            public const string Database = "database";
        }

        public static class HttpContext
        {
            public const string NoTimeout = "NoTimeout";
        }
    }
}
