using POKA.Utils.Interfaces;

namespace POKA.Utils.Repositories
{
    public interface IDbSetRepository<TEntity> : IRepositoryQueryable<TEntity>
        where TEntity : class, IEntity
    {
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity[]> CreateRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(IObjectId id, TEntity entity, CancellationToken cancellationToken = default);
        Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    }
}
