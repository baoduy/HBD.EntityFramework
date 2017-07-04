namespace HBD.EntityFramework.DomainEvents
{
    /// <summary>
    /// This interface using for the collection purposed.
    /// The real event-handler should be inherited from IHandler<in TEvent>
    /// </summary>
    public interface IEventHandler { }

    public interface IEventHandler<in TEvent> : IEventHandler where TEvent : IDomainEvent
    {
        void Handle(TEvent events);
    }
}