using HBD.EntityFramework.DomainEvents;
using HBD.EntityFramework.Entities;
using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System.Collections.Generic;

namespace HBD.EntityFramework.Aggregates
{
    public abstract class Aggregate<TKey> : IAggregate, IEntity<TKey>
    {
        public bool IsLocking { get; private set; }
        private readonly List<IDomainEvent> _domainEvents;

        protected Aggregate() : this(default(TKey)) { }

        protected Aggregate(TKey id)
        {
            Id = id;
            _domainEvents = new List<IDomainEvent>();
        }

        public TKey Id { get; }

        public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

        public void RemoveEvent(IDomainEvent events) => _domainEvents.Remove(events);

        protected void AddDomainEvent([NotNull]IDomainEvent events)
        {
            Guard.ArgumentIsNotNull(events, nameof(events));
            _domainEvents.Add(events);
        }

        /// <summary>
        /// Lock the entity. This entity will be lock forever after Add or Updated to the Database.
        /// The AddOrUpdate method will return the new entity from Db instead using the old one.
        /// </summary>
        public void Lock()
        {
            IsLocking = true;
        }
    }

    public abstract class Aggregate : Aggregate<int>
    {
    }

    public abstract class NamedAggregate : Aggregate<string>
    {
    }
}