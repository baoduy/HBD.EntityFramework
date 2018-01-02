using HBD.Framework.Attributes;
using HBD.Framework.Core;
using System;

namespace HBD.EntityFramework.DomainEvents
{
    public class EventHandler<TEvent> : IEventHandler<TEvent> where TEvent : IDomainEvent
    {
        public EventHandler([NotNull]Action<TEvent> handleAction)
        {
            Guard.ArgumentIsNotNull(handleAction, nameof(handleAction));
            HandleAction = handleAction;
        }

        public Action<TEvent> HandleAction { get; }

        public void Handle(TEvent events) => HandleAction.Invoke(events);
    }
}