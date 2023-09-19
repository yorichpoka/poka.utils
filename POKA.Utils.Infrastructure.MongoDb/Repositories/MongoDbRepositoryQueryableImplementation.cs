using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using System.Linq.Expressions;
using POKA.Utils.Repositories;
using POKA.Utils.Interfaces;
using MongoDB.Driver.Linq;
using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.Repositories
{
    public class MongoDbRepositoryQueryableImplementation<TEntity, TDatabase> : IRepositoryQueryable<TEntity>
        where TEntity : class, IEntity
        where TDatabase : BaseMongoDatabase
    {
        protected readonly IMongoCollection<TEntity> _mongoCollection;
        protected readonly TDatabase _database;

        public MongoDbRepositoryQueryableImplementation(TDatabase database, ICollectionNameProvider collectionNameProvider)
        {
            this._database = database;
            this._mongoCollection = this._database.MongoDatabase.GetCollection<TEntity>(collectionNameProvider.GetCollectionName<TEntity>());
        }

        public async Task<TDestination?> FirstOrDefaultMappedAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        {
            var result = await
                            this._mongoCollection
                                .AsQueryable()
                                .Where(predicate)
                                .Select(projection)
                                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<TDestination?> FirstOrDefaultMappedAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        {
            var result = await
                            this._mongoCollection
                                .AsQueryable()
                                .Select(projection)
                                .FirstOrDefaultAsync(cancellationToken);

            return result;
        }


        public async Task<List<TDestination>> GetMappedAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        {
            var result = await
                            this._mongoCollection
                                .AsQueryable()
                                .Where(predicate)
                                .Select(projection)
                                .ToListAsync(cancellationToken);

            return result;
        }

        public Task<List<TDestination>> GetMappedAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default) =>
            this.GetMappedAsync(l => true, projection, cancellationToken);


        public async Task<TDestination> MaxAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        {
            var data = await
                            this._mongoCollection
                                .AsQueryable()
                                .Where(predicate)
                                .Select(projection)
                                .ToListAsync(cancellationToken);

            var result = data.Any()
                            ? data.Max()
                            : default;

            return result;
        }

        public async Task<TDestination> MinAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default)
        {
            var data = await
                            this._mongoCollection
                                .AsQueryable()
                                .Where(predicate)
                                .Select(projection)
                                .ToListAsync(cancellationToken);

            var result = data.Any()
                            ? data.Min()
                            : default;

            return result;
        }


        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var cursor = await this._mongoCollection
                                    .FindAsync(predicate ?? (l => true), null, cancellationToken);

            var result = await cursor.FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var cursor = await this._mongoCollection
                                    .FindAsync(predicate ?? (l => true), null, cancellationToken);

            var result = await cursor.ToListAsync(cancellationToken);

            return result;
        }


        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            var result = await this._mongoCollection
                                    .CountDocumentsAsync(predicate ?? (l => true), cancellationToken: cancellationToken);

            return result > 0;
        }

        public Task<long> CountQueryAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
            (query as IMongoQueryable<T>).LongCountAsync(cancellationToken);


        public Task<List<T>> ExecuteQueryAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default) =>
            (query as IMongoQueryable<T>).ToListAsync(cancellationToken);


        public List<T> ExecuteQuery<T>(IQueryable<T> query) =>
            (query as IMongoQueryable<T>).ToList();

        public IQueryable<TEntity> AsQueryable() =>
            this._mongoCollection.AsQueryable();
    }
}
