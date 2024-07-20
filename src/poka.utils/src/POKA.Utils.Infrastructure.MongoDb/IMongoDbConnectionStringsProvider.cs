namespace POKA.Utils.Infrastructure.MongoDb
{
    public interface IMongoDbConnectionStringsProvider
    {
        bool AllowUsingOfTransaction { get; }
        (string connectionString, string dataBaseName) EventStore { get; }
        (string connectionString, string dataBaseName) Master { get; }
    }
}
