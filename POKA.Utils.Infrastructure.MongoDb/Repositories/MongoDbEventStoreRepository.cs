using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using POKA.Utils.Repositories;
using POKA.Utils.ValueObjects;
using POKA.Utils.Extensions;
using POKA.Utils.Interfaces;
using POKA.Utils.Entities;
using Newtonsoft.Json;
using POKA.Utils.DTOs;
using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.Repositories
{
    public class MongoDbEventStoreRepository : IEventStoreRepository
    {
        private static readonly List<Type> _domainTypes = AppDomain
                                                            .CurrentDomain
                                                            .GetAssemblies()
                                                            .SelectMany(l => l.GetTypes())
                                                            .Where(l => typeof(IDomainEvent<EventId>).IsAssignableFrom(l))
                                                            .ToList();
        private readonly IMongoCollection<EventEntity> _mongoCollection;
        private const int _snapshotSize = 100;

        public MongoDbEventStoreRepository(EventStoreMongoDatabase database, ICollectionNameProvider collectionNameProvider)
        {
            this._mongoCollection = database.MongoDatabase.GetCollection<EventEntity>(collectionNameProvider.GetCollectionName<EventEntity>());
        }

        public async IAsyncEnumerable<DomainEventDTO<TObjectId>> GetAsync<TObjectId>(
            IObjectId? aggregateId = null,
            string? aggregateType = null,
            string[]? excludedTypes = null,
            string[]? includedTypes = null,
            int? fromVersion = null,
            Period? period = null,
            CancellationToken cancellationToken = default
        ) where TObjectId : BaseObjectId
        {
            var filterBuilder = Builders<EventEntity>.Filter;
            var filterDefinition = filterBuilder.Empty;

            if (aggregateId is not null)
            {
                filterDefinition &= filterBuilder.Eq(l => l.AggregateId, aggregateId);
            }

            if (aggregateType is not null && aggregateType.HasValue())
            {
                filterDefinition &= filterBuilder.Eq(l => l.AggregateType, aggregateType);
            }

            if (excludedTypes is not null)
            {
                filterDefinition &= filterBuilder
                                        .Not(filterBuilder.In(l => l.Type, excludedTypes));
            }

            if (includedTypes is not null)
            {
                filterDefinition &= filterBuilder.In(l => l.Type, includedTypes);
            }

            if (fromVersion.HasValue)
            {
                filterDefinition &= filterBuilder.Gte(l => l.Version, fromVersion);
            }

            if (period is not null)
            {
                filterDefinition &= filterBuilder
                                        .And(
                                            filterBuilder.Gte(l => l.CreatedOn, period.From),
                                            filterBuilder.Lte(l => l.CreatedOn, period.To)
                                        );
            }

            var cursor = await this._mongoCollection.FindAsync(filterDefinition, null, cancellationToken: cancellationToken);

            var eventEntities = await cursor.ToListAsync(cancellationToken);

            foreach (var eventEntity in eventEntities)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                var eventType = _domainTypes.FirstOrDefault(l => l.Name == eventEntity.Type);

                if (eventType is null)
                {
                    throw new KeyNotFoundException(eventEntity.GetType().Name);
                }

                var domainEvent = JsonConvert.DeserializeObject(eventEntity.Data, eventType, Constants.DefaultJsonSerializerSettings) as IDomainEvent<TObjectId>;

                //domainEvent.AssignId(eventEntity.AggregateId as object);

                var result = new DomainEventDTO<TObjectId>(
                    aggregateId: eventEntity.AggregateId,
                    createdOn: eventEntity.CreatedOn,
                    version: eventEntity.Version,
                    domainEvent: domainEvent,
                    id: eventEntity.Id
                );

                yield return result;
            }
        }

        public async Task SaveAsync<TObjectId>(TObjectId aggregateId, string aggregateType, IDomainEvent<TObjectId> domainEvent, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            var eventEntity = new EventEntity(
                data: JsonConvert.SerializeObject(domainEvent, Constants.DefaultJsonSerializerSettings),
                id: BaseObjectId.Create<EventId>(),
                type: domainEvent.GetType().Name,
                aggregateType: aggregateType,
                version: domainEvent.Version,
                createdOn: DateTime.UtcNow,
                aggregateId: aggregateId,
                isSnapshot: false
            );

            await this._mongoCollection.CreateAsync(eventEntity, null, cancellationToken);
        }

        public async Task<TAggregate> LoadAggregateAsync<TAggregate, TObjectId>(IObjectId aggregateId, CancellationToken cancellationToken)
            where TObjectId : BaseObjectId
            where TAggregate : class, IAggregateRoot<TObjectId>
        {
            var aggregate = Activator.CreateInstance<TAggregate>();
            var aggregateType = typeof(TAggregate).Name;
            var fromVersion = 0;

            var latestEventSnapshotEntity = this._mongoCollection
                                                .AsQueryable()
                                                .Where(l => l.IsSnapshot && l.AggregateId == aggregateId)
                                                .OrderByDescending(l => l.Version)
                                                .FirstOrDefault();

            if (latestEventSnapshotEntity is not null)
            {
                var eventType = _domainTypes.FirstOrDefault(l => l.Name == latestEventSnapshotEntity.Type);
                var domainEvent = JsonConvert.DeserializeObject(latestEventSnapshotEntity.Data, eventType, Constants.DefaultJsonSerializerSettings) as IDomainEvent<TObjectId>;
                aggregate.ApplyDomainEvent(domainEvent);

                fromVersion = aggregate.Version + 1;
            }

            await foreach (var domainEventDTO in GetAsync<TObjectId>(aggregateId, aggregateType, fromVersion: fromVersion, cancellationToken: cancellationToken))
            {
                aggregate.ApplyDomainEvent(domainEventDTO.DomainEvent);
            }

            if (aggregate.Version == 0)
            {
                aggregate = null;
            }

            return aggregate;
        }

        public async Task SaveFromAggregateAsync<TObjectId>(IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            if (aggregate is null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            var versionSnapshot = await this.ApplyAggregateSnapshotAsync(aggregate, cancellationToken);

            var eventEntities = aggregate
                                    .GetUncommittedDomainEvents()
                                    .Select(
                                        l => new EventEntity(
                                                data: JsonConvert.SerializeObject(l, Constants.DefaultJsonSerializerSettings),
                                                isSnapshot: versionSnapshot == l.Version,
                                                aggregateType: aggregate.GetType().Name,
                                                id: BaseObjectId.Create<EventId>(),
                                                aggregateId: aggregate.Id,
                                                createdOn: DateTime.UtcNow,
                                                type: l.GetType().Name,
                                                version: l.Version
                                            )
                                    )
                                    .ToList();

            await this._mongoCollection.InsertManyAsync(eventEntities, cancellationToken: cancellationToken);
        }

        private async Task<int?> ApplyAggregateSnapshotAsync<TObjectId>(IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            var versionSnapshot = new int?();

            var latestSnapshotVersion =
                this._mongoCollection
                    .AsQueryable()
                    .Where(l => l.IsSnapshot && l.AggregateId == aggregate.Id)
                    .Select(l => l.Version)
                    .LastOrDefault();

            if (latestSnapshotVersion == 0)
            {
                if (aggregate.Version >= _snapshotSize)
                {
                    aggregate.Snapshot();
                    versionSnapshot = aggregate.Version;
                }
            }
            else
            {
                var numberVersionsOutdated = aggregate.Version - latestSnapshotVersion;

                if (numberVersionsOutdated >= _snapshotSize)
                {
                    aggregate.Snapshot();
                    versionSnapshot = aggregate.Version;
                }
            }

            return versionSnapshot;
        }
    }
}
