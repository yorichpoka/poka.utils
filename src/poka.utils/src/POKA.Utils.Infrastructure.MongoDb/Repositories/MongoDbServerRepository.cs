using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using POKA.Utils.Repositories;
using System.Linq.Expressions;
using POKA.Utils.Extensions;
using POKA.Utils.Interfaces;

namespace POKA.Utils.Infrastructure.MongoDb.Repositories
{
    public class MongoDbServerRepository<TEntity> : MongoDbRepositoryQueryableImplementation<TEntity, MasterMongoDatabase>, IDbSetRepository<TEntity>
        where TEntity : class, IEntity
    {
        public MongoDbServerRepository(MasterMongoDatabase database, ICollectionNameProvider collectionNameProvider)
            : base(database, collectionNameProvider)
        {
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default) =>
            this._mongoCollection
                .DeleteManyAsync(
                    session: this._database.ClientSessionHandle,
                    filter: predicate,
                    cancellationToken: cancellationToken
                );

        public async Task<TEntity[]> CreateRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
        {
            await
                this._mongoCollection
                    .InsertManyAsync(
                        session: this._database.ClientSessionHandle,
                        documents: entities,
                        cancellationToken: cancellationToken
                    );

            return entities;
        }

        public Task UpdateAsync(IObjectId id, TEntity entity, CancellationToken cancellationToken = default) =>
            this._mongoCollection.UpdateAsync(id, entity, this._database.ClientSessionHandle, cancellationToken);

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await
                this._mongoCollection
                    .CreateAsync(
                        clientSessionHandle: this._database.ClientSessionHandle,
                        entity: entity,
                        cancellationToken: cancellationToken
                    );

            return entity;
        }
    }
}
