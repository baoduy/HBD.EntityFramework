#region using

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace HBD.EntityFramework.Core
{
    public interface IEntity
    {
        [Required]
        [MaxLength(255)]
        string CreatedBy { get; set; }

        [Required]
        DateTime CreatedTime { get; set; }

        [MaxLength(255)]
        string UpdatedBy { get; set; }

        DateTime? UpdatedTime { get; set; }

        object[] GetKeys();
    }

    public interface IEntity<TKey> : IEntity
    {
        [Key]
        TKey Id { get; set; }
    }
}