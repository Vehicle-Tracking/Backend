using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Avt.Web.Backend.Data.Spec
{
    public interface IEntity<T> : IValidatableObject
    { 
        [Key]
        [Column(Order = 1)]
        T Id { get; set; }
    }
}
