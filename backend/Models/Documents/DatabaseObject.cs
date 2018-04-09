using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Documents
{
    public class DatabaseObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "v")]
        public int Version { get; set; }

        [JsonProperty(PropertyName = "d")]
        public string Data { get; set; }
    }
}
