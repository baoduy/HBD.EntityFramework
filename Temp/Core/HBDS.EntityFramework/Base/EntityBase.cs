#region using

using System;

#endregion

namespace HBDS.EntityFramework.Base
{
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        public object IdEntity => Id;
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedTime { get; set; }
        public virtual TKey Id { get; set; }
    }

    public abstract class EntityBase : EntityBase<int>
    {
    }
}