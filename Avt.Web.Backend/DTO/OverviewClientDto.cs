using System;
using Avt.Web.Backend.Data.Entities;
using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class OverviewClientDto
    {
        //public string vin { get; set; }
        //public string regNo { get; set; }
        //public string ownerName { get; set; }

        [JsonProperty("vehicle")]
        public VehicleClientDto Vehicle { get; set; }

        [JsonProperty("availiblity")]
        public float Availiblity { get; set; }

        [JsonProperty("lastStatus")]
        public int LastStatus { get; set; }

        [JsonProperty("lastSync")]
        public string LastSync { get; set; }

        [JsonProperty("lastSyncFormetted")]
        public string LastSyncStr { get; set; }
    }
}