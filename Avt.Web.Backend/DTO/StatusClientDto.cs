using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class StatusClientDto
    {
        [JsonProperty("vin")]
        public string VehicleId { get; set; }

        [JsonProperty("regNumber")]
        public string RegNumber { get; set; }

        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty("ownerAddress")]
        public string OwnerAddress { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("statusDateFormetted")]
        public string StatusDate { get; set; }

        [JsonProperty("statusDate")]
        public long StatusDateNumber { get; set; }
    }
}
