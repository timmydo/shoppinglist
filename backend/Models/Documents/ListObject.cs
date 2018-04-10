using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Documents
{
    public class ListObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "i")]
        public IList<ListItemObject> Items { get; set; }
    }
}
