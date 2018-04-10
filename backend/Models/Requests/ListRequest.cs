using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Requests
{
    public class ListRequest
    {
        [JsonProperty(PropertyName = "g")]
        public IList<string> Lists { get; set; }

        [JsonProperty(PropertyName = "m")]
        public IList<MarkRequest> Marks { get; set; }
    }
}
