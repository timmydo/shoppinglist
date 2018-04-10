using Newtonsoft.Json;

namespace backend.Models.Documents
{
    public class ListItemObject
    {
        [JsonProperty(PropertyName = "n")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "s")]
        public ListItemStates State { get; set; }
    }
}