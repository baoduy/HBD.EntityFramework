using HBD.EntityFramework.Core;

namespace HBD.EntityFramework.DomainEvents
{
    public class DtbDomainEvent<TDto> : IDomainEvent where TDto : IDto
    {
        public DtbDomainEvent(TDto data)
        {
            Data = data;
        }

        public TDto Data { get; }
    }
}