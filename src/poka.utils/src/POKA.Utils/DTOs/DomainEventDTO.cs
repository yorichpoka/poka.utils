using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;

namespace POKA.Utils.DTOs
{
    public class DomainEventDTO<TObjectId>
        where TObjectId : BaseObjectId
    {
        public EventId Id { get; private set; }
        public IObjectId AggregateId { get; private set; } = null!;
        public IDomainEvent<TObjectId> DomainEvent { get; private set; } = null!;
        public DateTime CreatedOn { get; private set; }
        public int Version { get; private set; }

        public DomainEventDTO()
        {
        }

        public DomainEventDTO(EventId id, IObjectId aggregateId, IDomainEvent<TObjectId> domainEvent, DateTime createdOn, int version)
        {
            AggregateId = aggregateId;
            DomainEvent = domainEvent;
            CreatedOn = createdOn;
            Version = version;
            Id = id;
        }
    }
}
