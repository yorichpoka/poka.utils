using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.DbContexts
{
    public class RequestStoreMongoDatabase : BaseMongoDatabase
    {
        public RequestStoreMongoDatabase(IMongoDatabase mongoDatabase, bool allowUsingOfTransaction = false) 
            : base(mongoDatabase, allowUsingOfTransaction)
        {
        }
    }
}
