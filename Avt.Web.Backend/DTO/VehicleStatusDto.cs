using System.Security.Policy;
using Avt.Web.Backend.Data.Types;
using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class VehicleStatusDto
    {
        [JsonProperty("vin")]
        public string VehicleId { get; set; }

        [JsonProperty("status")]
        public VehicleStatus VehicleStatus { get; set; }

        [JsonProperty("date")]
        public string StatusDate { get; set; }
    }
}
