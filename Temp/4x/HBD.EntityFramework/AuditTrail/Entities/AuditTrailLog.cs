using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace HBD.EntityFramework.AuditTrail.Entities
{
    public class AuditTrailLog
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [StringLength(150)]
        [Required]
        public virtual string UserName { get; set; }

        [Required]
        public virtual DateTime AuditDate { get; set; }

        [Required]
        public virtual EntityState ActionType { get; set; }

        [Required]
        [StringLength(256)]
        public virtual string ResordId { get; set; }

        [StringLength(100)]
        [Required]
        public virtual string TableName { get; set; }

        public virtual HashSet<AuditTrailDetails> Details { get; set; } = new HashSet<AuditTrailDetails>();
    }
}