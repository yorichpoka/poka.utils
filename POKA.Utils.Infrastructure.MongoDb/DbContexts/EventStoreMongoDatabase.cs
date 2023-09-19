using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.DbContexts
{
    public class EventStoreMongoDatabase : BaseMongoDatabase
    {
        public EventStoreMongoDatabase(IMongoDatabase mongoDatabase, bool allowUsingOfTransaction = false) 
            : base(mongoDatabase, allowUsingOfTransaction)
        {
        }
    }
}
