using backend.Models.Documents;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace backend.Models.Requests
{
    public class UserRequest
    {
        [JsonProperty(PropertyName = "a")]
        public IList<ListDescriptorObject> ListsToAdd { get; set; }

        [JsonProperty(PropertyName = "r")]
        public IList<ListDescriptorObject> ListsToRemove { get; set; }
    }
}
