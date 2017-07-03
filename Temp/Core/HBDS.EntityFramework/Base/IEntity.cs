#region using

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace HBDS.EntityFramework.Base
{
    public interface IEntity
    {
        [NotMapped]
        object IdEntity { get; }

        [Required]
        [MaxLength(255)]
        string CreatedBy { get; set; }

        [Required]
        DateTime CreatedTime { get; set; }

        [MaxLength(255)]
        string UpdatedBy { get; set; }

        DateTime? UpdatedTime { get; set; }
    }

    public interface IEntity<TKey> : IEntity
    {
        [Key]
        TKey Id { get; set; }
    }
}