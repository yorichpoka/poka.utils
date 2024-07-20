using POKA.Utils.Interfaces;
using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.DbContexts
{
    public class BaseMongoDatabase : IUnitOfWork
    {
        private IClientSessionHandle? _clientSessionHandle;
        private int _clientSessionHandleCount;
        private bool _allowUsingOfTransactions;
        public IClientSessionHandle? ClientSessionHandle => this._clientSessionHandle;
        public IMongoDatabase MongoDatabase { get; }

        protected BaseMongoDatabase(IMongoDatabase mongoDatabase, bool allowUsingOfTransaction = false)
        {
            _allowUsingOfTransactions = allowUsingOfTransaction;
            MongoDatabase = mongoDatabase;
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this._clientSessionHandle is null)
            {
                return;
            }

            if (this._allowUsingOfTransactions)
            {
                await this._clientSessionHandle.AbortTransactionAsync(cancellationToken);
            }

            this._clientSessionHandle = null;
            this._clientSessionHandleCount = 0;
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (this._clientSessionHandle is null)
            {
                return;
            }

            this._clientSessionHandleCount--;

            if (this._clientSessionHandleCount == 0)
            {
                if (this._allowUsingOfTransactions)
                {
                    await this._clientSessionHandle.CommitTransactionAsync(cancellationToken);
                }

                this._clientSessionHandle = null;
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            this._clientSessionHandleCount++;

            if (this._clientSessionHandle is not null)
            {
                return;
            }

            this._clientSessionHandle = await this.MongoDatabase.Client.StartSessionAsync(null, cancellationToken);

            if (this._allowUsingOfTransactions)
            {
                this._clientSessionHandle.StartTransaction();
            }
        }
    }
}
