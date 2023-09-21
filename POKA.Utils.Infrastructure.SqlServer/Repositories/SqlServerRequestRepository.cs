using POKA.Utils.Infrastructure.SqlServer.DbContexts;
using System.Linq.Expressions;
using MediatR;

namespace POKA.Utils.Infrastructure.SqlServer.Repositories
{
    public class SqlServerRequestRepository : IRequestRepository
    {
        private readonly IApplicationNameProvider _applicationNameProvider;
        private readonly ICurrentUserIdProvider _currentUserIdProvider;
        private readonly SqlEventStoreDbContext _dbContext;
        private RequestScopeId _scopeId;
        private RequestId? _parentId;

        public SqlServerRequestRepository(SqlEventStoreDbContext dbContext, ICurrentUserIdProvider currentUserIdProvider, IApplicationNameProvider applicationNameProvider)
        {
            _scopeId = BaseObjectId.Create<RequestScopeId>();
            _applicationNameProvider = applicationNameProvider;
            _currentUserIdProvider = currentUserIdProvider;
            _dbContext = dbContext;
        }

        public async Task<RequestEntity> InitializeAsync(IBaseRequest request, CancellationToken cancellationToken = default)
        {
            var requestEntity = new RequestEntity(
                data: JsonConvert.SerializeObject(request, Constants.DefaultJsonSerializerSettings),
                applicationPerformer: this._applicationNameProvider.ApplicationName,
                createdByUserId: this._currentUserIdProvider?.Id,
                name: request.GetType().Name,
                parentId: this._parentId,
                scopeId: _scopeId
            );

            if (request is IBaseCommand)
            {
                requestEntity.AsCommand();
            }
            else
            {
                requestEntity.AsQuery();
            }

            await
                this._dbContext
                    .Set<RequestEntity>()
                    .AddAsync(requestEntity, cancellationToken);

            await this._dbContext.SaveChangesAsync(cancellationToken);

            this._parentId = requestEntity.Id;

            return requestEntity;
        }

        public async Task<long> CountAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var result = await this._dbContext
                                    .Set<RequestEntity>()
                                    .Where(predicate)
                                    .CountAsync(cancellationToken);

            return result;
        }

        public async Task DeleteAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            var dbSet = this._dbContext.Set<RequestEntity>();
            var query = dbSet
                            .Where(predicate)
                            .Select(l => new { l.Id })
                            .AsQueryable();

            var queryResult = await query.ToListAsync(cancellationToken);

            var requestEntities = queryResult.Select(l => new RequestEntity(l.Id))
                                             .ToArray();

            dbSet.RemoveRange(requestEntities);

            await this._dbContext.SaveChangesAsync(cancellationToken);
        }

        public Task UpdateAsync(RequestId requestId, RequestEntity request, CancellationToken cancellationToken = default) =>
            this._dbContext.SaveChangesAsync(cancellationToken);
    }
}
