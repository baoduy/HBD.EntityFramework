#region using

using System;
using System.ComponentModel.DataAnnotations;

#endregion using

namespace HBD.EntityFramework.Core
{
    public interface IDbEntity
    {
        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get;}

        object[] GetKeys();
    }

    /// <summary>
    /// DbEntity is representative for EntityFramework entities.
    /// </summary>
    public interface IDbEntity<TKey, TAuditKey> : IDbEntity
    {
        [Key]
        TKey Id { get; set; }

        [Required]
        [MaxLength(255)]
        TAuditKey CreatedBy { get; set; }

        [Required]
        DateTime CreatedOn {get; set; }

        [MaxLength(255)]
        TAuditKey UpdatedBy { get; set; }

        DateTime? UpdatedOn { get; set; }
    }
}