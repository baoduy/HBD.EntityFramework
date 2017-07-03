#region using

using System;

#endregion

namespace HBD.EntityFramework.Core
{
    public abstract class Entity<TKey> : IEntity<TKey>
    {
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual string UpdatedBy { get; set; }
        public virtual DateTime? UpdatedTime { get; set; }
        public virtual TKey Id { get; set; }

        public virtual object[] GetKeys() => new object[] { Id };
    }

    public abstract class Entity : Entity<int>
    {
    }

    public abstract class NamedEntity : Entity<string>
    {
    }
}