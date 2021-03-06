﻿#region using

using HBD.EntityFramework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public abstract class DbEntity<TKey> : IDbEntity<TKey, string>, IValidatableObject
    {
        protected DbEntity()
        {
            CreatedOn = DateTime.Now;
        }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        public virtual byte[] RowVersion { get;private set; }

        public virtual object[] GetKeys() => new object[] { Id };

        /// <inheritdoc />
        /// <summary>
        /// Apply the custom validation for this entity.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            => new ValidationResult[] { };
    }

    public abstract class DbEntity : DbEntity<int>
    {

    }
}