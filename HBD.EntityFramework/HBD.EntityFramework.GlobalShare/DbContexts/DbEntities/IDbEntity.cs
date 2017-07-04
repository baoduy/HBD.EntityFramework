﻿#region using

using System;
using System.ComponentModel.DataAnnotations;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public interface IDbEntity
    {
        [Timestamp]
        [ConcurrencyCheck]
        byte[] RowVersion { get; set; }

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
        DateTime CreatedTime { get; set; }

        [MaxLength(255)]
        TAuditKey UpdatedBy { get; set; }

        DateTime? UpdatedTime { get; set; }
    }
}