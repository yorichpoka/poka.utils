using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;

namespace POKA.Utils.Entities
{
    public class EventEntity : BaseEntity<EventId>, IHasCreatedOn, IHasVersion
    {
        public BaseObjectId AggregateId { get; private set; } = null!;
        public string AggregateType { get; private set; } = null!;
        public string Type { get; private set; } = null!;
        public string Data { get; private set; } = null!;
        public bool IsSnapshot { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public int Version { get; private set; }

        private EventEntity()
        {
        }

        public EventEntity(BaseObjectId aggregateId, string aggregateType, string type, string data, bool isSnapshot, int version)
        {
            Id = BaseObjectId.Create<EventId>();
            CreatedOn = DateTime.UtcNow;
            AggregateType = aggregateType;
            AggregateId = aggregateId;
            IsSnapshot = isSnapshot;
            Version = version;
            Type = type;
            Data = data;
        }
    }
}
