using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Avt.Web.Backend.Data.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Avt.Web.Backend.Data.Entities
{
    public class Vehicle: EntityBase<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required, MaxLength(256)]
        public override string Id { get; set; }

        [Required, MaxLength(256)]
        public string RegNumber { get; set; }

        public string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public Owner Owner { get; set; }
        public ICollection<VehicleStatusDetail> StatusDetails { get; set; }
    }
}