using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models.Responses
{
    public class ListResponse
    {
        [JsonProperty(PropertyName = "lists")]
        public IList<string> Lists { get; set; } = new List<string>();

    }
}
