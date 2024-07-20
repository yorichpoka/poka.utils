using POKA.Utils.ValueObjects;
using POKA.Utils.Interfaces;
using POKA.Utils.DTOs;

namespace POKA.Utils.Repositories
{
    public interface IEventStoreRepository
    {
        Task<TAggregate> LoadAggregateAsync<TAggregate, TObjectId>(IObjectId aggregateId, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
            where TAggregate : class, IAggregateRoot<TObjectId>;

        Task SaveAsync<TObjectId>(TObjectId aggregateId, string aggregateType, IDomainEvent<TObjectId> domainEvent, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId;

        Task SaveFromAggregateAsync<TObjectId>(IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId;

        IAsyncEnumerable<DomainEventDTO<TObjectId>> GetAsync<TObjectId>(
            IObjectId? aggregateId = null,
            string? aggregateType = null,
            string[]? excludedTypes = null,
            string[]? includedTypes = null,
            int? fromVersion = null,
            Period? period = null,
            CancellationToken cancellationToken = default
        ) where TObjectId : BaseObjectId;
    }
}
