using HBD.EntityFramework.Aggregates;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HBD.EntityFramework.DomainEvents
{
    public static class DomainEventsDispatcher
    {
        private static readonly IList<IEventHandler> _handlers = new List<IEventHandler>();

        public static void Register(IEventHandler handler)
        {
            _handlers.Add(handler);
        }

        internal static void RaiseEvents(IAggregate aggregate)
        {
            while (aggregate.DomainEvents.Count > 0)
            {
                var events = aggregate.DomainEvents.First();
                Dispatch(events);
                aggregate.RemoveEvent(events);
            }
        }

        public static void Dispatch(IDomainEvent domainEvent)
        {
            foreach (var handler in _handlers)
            {
                bool canHandleEvent = handler.GetType().GetTypeInfo().GetInterfaces()
                    .Any(x => x.GetTypeInfo().IsGenericType
                        && x.GetGenericTypeDefinition() == typeof(IEventHandler<>)
                        && x.GenericTypeArguments[0] == domainEvent.GetType());

                if (canHandleEvent)
                {
                    dynamic h = handler;
                    h.Handle((dynamic)domainEvent);
                }
            }
        }
    }
}