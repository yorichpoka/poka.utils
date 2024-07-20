namespace POKA.Utils.Infrastructure.MongoDb
{
    public interface ICollectionNameProvider
    {
        string GetCollectionName<TEntity>();
    }
}
