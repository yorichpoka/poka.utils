using POKA.Utils.Interfaces;

namespace POKA.Utils.Repositories
{
    public interface IRepositoryQueryable<TEntity>
        where TEntity : class, IEntity
    {
        Task<TDestination?> FirstOrDefaultMappedAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);
        Task<TDestination?> FirstOrDefaultMappedAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

        Task<List<TDestination>> GetMappedAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);
        Task<List<TDestination>> GetMappedAsync<TDestination>(Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

        Task<TDestination> MaxAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);
        Task<TDestination> MinAsync<TDestination>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TDestination>> projection, CancellationToken cancellationToken = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);
        Task<long> CountQueryAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<List<T>> ExecuteQueryAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);
        List<T> ExecuteQuery<T>(IQueryable<T> query);
        IQueryable<TEntity> AsQueryable();
    }
}
