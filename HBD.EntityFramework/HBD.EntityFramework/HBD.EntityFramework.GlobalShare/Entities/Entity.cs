using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBD.EntityFramework.Entities
{
    public abstract class Entity<TKey> : IEntity<TKey>, IEquatable<IEntity<TKey>>, IValidatableObject
    {
        protected Entity(TKey id)
        {
            Id = id;
        }

        public TKey Id { get; protected set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null)) return false;

            if (obj is IEntity<TKey> b)
                return Equals(b);

            return false;
        }

        public abstract bool Equals(IEntity<TKey> other);

        public override int GetHashCode() => GetType().FullName.GetHashCode() * 397 ^ Id.GetHashCode();

        /// <summary>
        /// Apply the custom validation here.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Apply the custom validation here.
            return new ValidationResult[] { };
        }
    }

    public abstract class Entity : Entity<int>
    {
        protected Entity(int id) : base(id)
        { }

        public override bool Equals(IEntity<int> other)
        {
            if (GetType() != other.GetType()) return false;
            return Id == other.Id;
        }
    }

    public abstract class NamedEntity : Entity<string>
    {
        protected NamedEntity(string id) : base(id)
        { }

        public override bool Equals(IEntity<string> other)
        {
            if (GetType() != other.GetType()) return false;
            return Id == other.Id;
        }
    }
}