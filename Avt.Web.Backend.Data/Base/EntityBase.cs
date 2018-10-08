using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Avt.Web.Backend.Data.Spec;

namespace Avt.Web.Backend.Data.Base
{
    public abstract class EntityBase<T> : IEntity<T>
    {
        [Key]
        [Column(Order = 1)]
        public abstract T Id { get; set; }

        public virtual DateTime? CreationDate { get; set; } = DateTime.UtcNow;

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return new ValidationResult[0];
        }
    }
}
