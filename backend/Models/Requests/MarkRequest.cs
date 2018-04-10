using backend.Models.Documents;
using Newtonsoft.Json;

namespace backend.Models.Requests
{
    public class MarkRequest
    {
        [JsonProperty(PropertyName = "r")]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "l")]
        public string ListId { get; set; }

        [JsonProperty(PropertyName = "i")]
        public ListItemObject Item { get; set; }
    }
}
