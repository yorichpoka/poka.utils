using POKA.Utils.Infrastructure.SqlServer.DbContexts;

namespace POKA.Utils.Infrastructure.SqlServer.Repositories
{
    public class SqlEventStoreRepository : IEventStoreRepository
    {
        private static List<Type> _domainTypes = typeof(IDomainEvent<>)
                                                    .Assembly
                                                    .GetTypes()
                                                    .Where(l => !l.IsInterface)
                                                    .ToList();

        private readonly SqlEventStoreDbContext _databaseContext;

        public SqlEventStoreRepository(SqlEventStoreDbContext databaseContext)
        {
            _databaseContext = databaseContext;
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
            var query = this._databaseContext
                            .Set<EventEntity>()
                            .AsQueryable();

            if (aggregateId.HasValue())
            {
                query = query.Where(l => l.AggregateId == aggregateId.Value.ToString());
            }

            if (aggregateType.HasValue())
            {
                query = query.Where(l => l.AggregateType == aggregateType);
            }

            if (fromVersion.HasValue)
            {
                query = query.Where(l => l.Version >= fromVersion.Value);
            }

            if (excludedTypes is not null)
            {
                query = query.Where(l => !excludedTypes.Contains(l.Type));
            }

            if (includedTypes is not null)
            {
                query = query.Where(l => includedTypes.Contains(l.Type));
            }

            await foreach (var eventEntity in query.AsAsyncEnumerable())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                var eventType = _domainTypes.FirstOrDefault(l => l.Name == eventEntity.Type);

                if (eventType is null)
                {
                    throw new KeyNotFoundException(eventEntity.Type);
                }

                var domainEvent = JsonConvert.DeserializeObject(eventEntity.Data, eventType, Constants.DefaultJsonSerializerSettings) as IDomainEvent<TObjectId>;

                domainEvent.AssignId(BaseObjectId.Parse<TObjectId>(eventEntity.AggregateId));

                var dto = new DomainEventDTO<TObjectId>
                (
                    aggregateId: BaseObjectId.Parse<TObjectId>(eventEntity.AggregateId),
                    createdOn: eventEntity.CreatedOn,
                    version: eventEntity.Version,
                    domainEvent: domainEvent,
                    id: eventEntity.Id
                );

                yield return dto;
            }
        }

        public async Task SaveAsync<TObjectId>(TObjectId aggregateId, string aggregateType, IDomainEvent<TObjectId> domainEvent, CancellationToken cancellationToken = default) 
            where TObjectId : BaseObjectId
        {
            var eventEntity = new EventEntity(
                                    data: JsonConvert.SerializeObject(domainEvent, Constants.DefaultJsonSerializerSettings),
                                    aggregateId: aggregateId.Value.ToString(),
                                    aggregateType: aggregateType,
                                    type: domainEvent.GetType().Name,
                                    version: domainEvent.Version
                                );

            await
                this._databaseContext
                    .Set<EventEntity>()
                    .AddAsync(eventEntity, cancellationToken);

            await this._databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task SaveFromAggregateAsync<TObjectId>(IAggregateRoot<TObjectId> aggregate, CancellationToken cancellationToken = default)
            where TObjectId : BaseObjectId
        {
            if (aggregate is null)
            {
                throw new ArgumentNullException(nameof(aggregate));
            }

            var eventEntities = aggregate
                                    .GetUncommittedDomainEvents()
                                    .Select(
                                        l => new EventEntity(
                                            data: JsonConvert.SerializeObject(l, Constants.DefaultJsonSerializerSettings),
                                            aggregateId: aggregate.Id.Value.ToString(),
                                            aggregateType: aggregate.GetType().Name,
                                            type: l.GetType().Name,
                                            version: l.Version
                                        )
                                    )
                                    .ToList();

            await
                this._databaseContext
                    .Set<EventEntity>()
                    .AddRangeAsync(eventEntities, cancellationToken);

            await this._databaseContext.SaveChangesAsync(cancellationToken);
        }

        public async Task WipeAsync(string aggregateId, string aggregateType, CancellationToken cancellationToken = default)
        {
            var dbSet = this._databaseContext.Set<EventEntity>();
            var eventEntities = await
                                    dbSet
                                        .Where(
                                            l => l.AggregateId == aggregateId &&
                                                 l.AggregateType == aggregateType
                                        )
                                        .ToListAsync(cancellationToken);

            if (eventEntities.Any())
            {
                dbSet.RemoveRange(eventEntities);
                await this._databaseContext.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<TAggregate> LoadAggregateAsync<TAggregate, TObjectId>(IObjectId aggregateId, CancellationToken cancellationToken)
            where TAggregate : class, IAggregateRoot<TObjectId> 
            where TObjectId : BaseObjectId
        {
            var aggregate = Activator.CreateInstance<TAggregate>();
            var aggregateType = typeof(TAggregate).Name;
            var fromVersion = 0;

            var latestEventSnapshotEntity = this._databaseContext
                                                .Set<EventEntity>()
                                                .AsQueryable()
                                                .Where(l => l.IsSnapshot && l.AggregateId == aggregateId.Value.ToString())
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
    }
}
