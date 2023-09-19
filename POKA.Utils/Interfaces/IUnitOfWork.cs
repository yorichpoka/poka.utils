namespace POKA.Utils.Interfaces
{
    public interface IUnitOfWork
    {
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    }
}
