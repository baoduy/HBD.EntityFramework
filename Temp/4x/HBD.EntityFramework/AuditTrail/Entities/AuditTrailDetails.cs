using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBD.EntityFramework.AuditTrail.Entities
{
    public partial class AuditTrailDetails
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        public virtual int AudiTrailId { get; set; }

        public virtual AuditTrailLog Log { get; set; }

        [Required]
        [StringLength(100)]
        public virtual string ColumnName { get; set; }

        [StringLength(int.MaxValue)]
        public virtual string OldValue { get; set; }

        [StringLength(int.MaxValue)]
        public virtual string NewValue { get; set; }
    }
}