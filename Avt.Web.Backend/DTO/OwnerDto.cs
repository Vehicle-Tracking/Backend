using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class OwnerDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    //    public string Address { get; set; }
    }
}