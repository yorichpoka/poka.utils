using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;

namespace POKA.Utils
{
    public abstract class BaseDomainEvent<TObjectId> : IDomainEvent<TObjectId>
        where TObjectId : BaseObjectId
    {
        public TObjectId Id { get; private set; }
        public int Version { get; private set; }
        public UserId? AuthorId { get; private set; }
        public DateTime On { get; private set; }

        protected BaseDomainEvent(TObjectId id)
        {
            On = DateTime.UtcNow;
            AuthorId = null;
            Version = 0;
            Id = id;
        }

        protected BaseDomainEvent(TObjectId id, int version, UserId? authorId = null)
        {
            On = DateTime.UtcNow;
            AuthorId = authorId;
            Version = version;
            Id = id;
        }

        public abstract string GetStreamName();

        public void AssignId(TObjectId id) => this.Id = id;
    }
}
