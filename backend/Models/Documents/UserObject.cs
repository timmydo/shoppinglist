using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Documents
{
    public class UserObject : IDocumentObject
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonIgnore]
        public string Etag { get; set; }

        [JsonProperty(PropertyName = "l")]
        public IList<ListDescriptorObject> Lists { get; set; }
    }
}
