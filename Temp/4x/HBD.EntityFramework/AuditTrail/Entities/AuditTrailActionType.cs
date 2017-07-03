using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.AuditTrail.Entities
{
    internal class AuditTrailActionType
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int Id { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string ActionName { get; set; }
    }
}