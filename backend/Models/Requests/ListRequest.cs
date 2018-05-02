using backend.Models.Documents;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Requests
{
    public class ListRequest
    {
        [JsonProperty(PropertyName = "g")]
        public IList<string> ListsToGet { get; set; }

        [JsonProperty(PropertyName = "m")]
        public IList<MarkRequest> Marks { get; set; }
    }
}
