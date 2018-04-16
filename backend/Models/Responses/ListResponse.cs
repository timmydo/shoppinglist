using backend.Models.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Responses
{
    public class ListResponse
    {
        [JsonProperty(PropertyName = "l")]
        public IList<ListData> Lists { get; set; } = new List<ListData>();

        [JsonProperty(PropertyName = "m")]
        public IList<MarkResponse> Marks { get; set; } = new List<MarkResponse>();

        public class ListData
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "i")]
            public IList<ListItemObject> Items { get; set; }
        }

        public class MarkResponse
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "s")]
            public bool Success { get; set; }

            [JsonProperty(PropertyName = "c")]
            public MarkResponseReasonCode ReasonCode { get; set; }
        }
    }
}
