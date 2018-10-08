using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Types;

namespace Avt.Web.Backend.Data.Entities
{
    public class VehicleAggregateOverview : EntityBase<string>
    {
        [Key]
        [MaxLength(256)]
        public override string Id { get; set; }

        [MaxLength(256)]
        public string RegNumber { get; set; }

        [MaxLength(256)]
        public string OwnerName { get; set; }
        public long Total { get; set; }
        public long ConnectedStatusCount { get; set; }
        public long DisconnectedStatusCount { get; set; }

        public VehicleStatus LastStatus { get; set; }
        public DateTime LastSync { get; set; }
    }

}
