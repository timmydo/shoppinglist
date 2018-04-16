using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Documents
{
    public class ListObject : IDocumentObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonIgnore]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "i")]
        public IList<ListItemObject> Items { get; set; }
    }
}
