using POKA.Utils.ValueObjects;

namespace POKA.Utils.Interfaces
{
    public interface IAggregateRoot<TObjectId> : IHasCreatedOn, IHasVersion, IHasCreatedByUserId, ICloneable
        where TObjectId : BaseObjectId
    {
        TObjectId Id { get; }
        IReadOnlyCollection<IDomainEvent<TObjectId>> GetUncommittedDomainEvents();
        IReadOnlyCollection<IDomainEvent<TObjectId>> GetDomainEvents();
        void ApplyDomainEvent(IDomainEvent<TObjectId> domainEvent);
        void CommitDomainEvents();
        void Snapshot();
    }
}
