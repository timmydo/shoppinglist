using backend.Models.Documents;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Responses
{
    public class GetMyAccountResponse
    {
        [JsonProperty(PropertyName = "l")]
        public IList<ListDescriptorObject> Lists { get; set; }
    }
}
