using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Avt.Web.Backend.Data.Base;

namespace Avt.Web.Backend.Data.Entities
{
    public class Owner : EntityBase<string>
    {
        [Required, MaxLength(256)]
        [Key]
        public override string Id { get; set; }

        [Required, MaxLength(1024)]
        public string Address { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
    }
}