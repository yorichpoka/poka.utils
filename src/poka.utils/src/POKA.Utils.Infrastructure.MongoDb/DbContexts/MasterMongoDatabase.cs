using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb.DbContexts
{
    public class MasterMongoDatabase : BaseMongoDatabase
    {
        public MongoClient MongoClient { get; }

        public MasterMongoDatabase(MongoClient mongoClient, IMongoDatabase mongoDatabase, bool allowUsingOfTransaction = false)
            : base(mongoDatabase, allowUsingOfTransaction)
        {
            MongoClient = mongoClient;
        }
    }
}
