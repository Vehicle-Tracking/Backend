using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class VehicleClientDto
    {

        [JsonProperty("vin")]
        public string VehicleId { get; set; }

        [JsonProperty("regNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("owner")]
        public OwnerDto Owner { get; set; }
    }
}