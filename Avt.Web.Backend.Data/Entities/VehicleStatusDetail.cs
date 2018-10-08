using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Avt.Web.Backend.Data.Base;
using Avt.Web.Backend.Data.Types;

namespace Avt.Web.Backend.Data.Entities
{
    public class VehicleStatusDetail : EntityBase<int>
    {
        [Required]
        public override int Id { get; set; }

        public VehicleStatus Status { get; set; }

        public DateTime SyncDate { get; set; }
        
        public string VehicleId { get; set; }

        [ForeignKey("VehicleId")]
        public Vehicle Vehicle { get; set; }
    }
}
