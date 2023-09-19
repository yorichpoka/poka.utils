using POKA.Utils.ValueObjects;
using POKA.Utils.Entities;

namespace POKA.Utils.Repositories
{
    public interface IRequestRepository
    {
        Task<long> CountAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task DeleteAsync(Expression<Func<RequestEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task UpdateAsync(RequestId requestId, RequestEntity request, CancellationToken cancellationToken = default);
        Task<RequestEntity> InitializeAsync(IBaseRequest request, CancellationToken cancellationToken = default);
    }
}
