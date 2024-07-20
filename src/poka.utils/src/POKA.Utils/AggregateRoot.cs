using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;

namespace POKA.Utils
{
    public abstract class AggregateRoot<TObjectId> : BaseEntity<TObjectId>, IAggregateRoot<TObjectId>
        where TObjectId : BaseObjectId
    {
        private readonly List<IDomainEvent<TObjectId>> _uncommittedDomainEvents = new();
        private readonly List<IDomainEvent<TObjectId>> _domainEvents = new();
        int? _requestedHashCode;

        public int Version { get; protected set; }
        public DateTime CreatedOn { get; protected set; }
        public UserId? CreatedByUserId { get; protected set; } = null;
        public DateTime LastUpdatedOn { get; protected set; }

        protected AggregateRoot()
            : base()
        {
        }

        protected void ApplyUncommittedDomainEvent(IDomainEvent<TObjectId> domainEvent) => ApplyDomainEventInternal(domainEvent, false);

        public IReadOnlyCollection<IDomainEvent<TObjectId>> GetUncommittedDomainEvents() => _uncommittedDomainEvents.AsReadOnly();

        public void ApplyDomainEvent(IDomainEvent<TObjectId> domainEvent) => ApplyDomainEventInternal(domainEvent, true);

        public IReadOnlyCollection<IDomainEvent<TObjectId>> GetDomainEvents() => _domainEvents.AsReadOnly();

        private void ApplyDomainEventInternal(IDomainEvent<TObjectId> domainEvent, bool committed = false)
        {
            if (this.IsChanged == false)
            {
                this.BeginChanges();
            }

            ApplyDomainEventImplementation(domainEvent);

            if (committed)
            {
                _domainEvents.Add(domainEvent);
            }
            else
            {
                _uncommittedDomainEvents.Add(domainEvent);
            }
        }

        public void CommitDomainEvents()
        {
            foreach (var uncommitedDomainEvent in this._uncommittedDomainEvents)
            {
                this._domainEvents.Add(uncommitedDomainEvent);
            }

            this._uncommittedDomainEvents.Clear();

            this.EndChanges();
        }

        public abstract void ApplyDomainEventImplementation(IDomainEvent<TObjectId> domainEvent);

        public object Clone() => MemberwiseClone();

        private bool IsTransient => Id.Value == default;

        public override bool Equals(object obj)
        {
            if (obj == null || obj is not AggregateRoot<TObjectId>)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var item = (AggregateRoot<TObjectId>)obj;

            if (item.IsTransient || IsTransient)
            {
                return false;
            }
            else
            {
                return item.Id == Id;
            }
        }

        public override int GetHashCode()
        {
            if (!IsTransient)
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = Id.GetHashCode() ^ 31;
                }

                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }

        public abstract void Snapshot();

        public static bool operator ==(AggregateRoot<TObjectId> left, AggregateRoot<TObjectId> right)
        {
            bool result;

            if (Equals(left, null))
            {
                result = Equals(right, null);
            }
            else
            {
                result = left.Equals(right);
            }

            return result;
        }

        public static bool operator !=(AggregateRoot<TObjectId> left, AggregateRoot<TObjectId> right) => !(left == right);

        public override string ToString() => $"{GetType().Name} [Id={Id}]";
    }
}
