namespace POKA.Utils.Infrastructure.MongoDb
{
    public interface IMongoDbConnectionStrings
    {
        bool AllowUsingOfTransaction { get; }
        (string connectionString, string dataBaseName) EventStore { get; }
        (string connectionString, string dataBaseName) Master { get; }
    }
}
