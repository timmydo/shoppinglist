using Newtonsoft.Json;

namespace backend.Models.Documents
{
    public class ListDescriptorObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "n")]
        public string Name { get; set; }
    }
}
