using System.Collections.Generic;
using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class FilterItemsDto
    {
        [JsonProperty("owners")]
        public List<Pair<string, string>> Owners { get; set; }

        [JsonProperty("allStatus")]
        public List<Pair<int, string>> AllStatus { get; set; }
    }
}