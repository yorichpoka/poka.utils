namespace POKA.Utils.Infrastructure.SqlServer
{
    public interface ISqlServerConnectionStringsProvider
    {
        string EventStoreDbConnectionString { get; }
        string MasterDbConnectionString { get; }
    }
}
