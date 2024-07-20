using POKA.Utils.Infrastructure.SqlServer.DbContexts;
using System.Linq.Expressions;

namespace POKA.Utils.Infrastructure.SqlServer.Repositories
{
    public class SqlServerDbSetRepository<TEntity> : SqlServerRepositoryQueryableImplementation<TEntity, SqlMasterDbContext>, IDbSetRepository<TEntity>
        where TEntity : class, IEntity
    {
        public SqlServerDbSetRepository(SqlMasterDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var entities = await
                                this._dbSet
                                    .Where(predicate)
                                    .ToListAsync(cancellationToken);

            this._dbSet.RemoveRange(entities);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TEntity[]> CreateRangeAsync(TEntity[] entities, CancellationToken cancellationToken = default)
        {
            await this._dbSet.AddRangeAsync(entities, cancellationToken);

            await this._dbContext.SaveChangesAsync(cancellationToken);

            return entities;
        }

        public Task UpdateAsync(IObjectId id, TEntity entity, CancellationToken cancellationToken = default) =>
            this._dbContext.SaveChangesAsync(cancellationToken);

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await this._dbSet.AddAsync(entity, cancellationToken);

            await this._dbContext.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
