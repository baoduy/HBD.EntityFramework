using HBD.EntityFramework.DomainEvents;
using HBD.EntityFramework.Entities;
using System.Collections.Generic;

namespace HBD.EntityFramework.Aggregates
{
    public interface IAggregate : IEntity
    {
        IReadOnlyList<IDomainEvent> DomainEvents { get; }

        void RemoveEvent(IDomainEvent events);
    }
}