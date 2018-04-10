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
        [JsonProperty(PropertyName = "lists")]
        public IList<ListData> Lists { get; set; } = new List<ListData>();

        public class ListData
        {
            [JsonProperty(PropertyName = "id")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "i")]
            public IList<ListItemObject> Items { get; set; }
        }
    }
}
