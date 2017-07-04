#region using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion using

namespace HBD.EntityFramework.DbContexts.DbEntities
{
    public abstract class DbEntity<TKey> : IDbEntity<TKey, string>, IValidatableObject
    {
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedTime { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }

        public virtual byte[] RowVersion { get; set; }

        public virtual object[] GetKeys() => new object[] { Id };

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