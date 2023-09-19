namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class MongoDBEntityTypeConfiguration
    {
        public static void Configure()
        {
            MongoDbConfiguration.Configure();
            RequestEntityTypeConfiguration.Configure();
            CommonEntityTypeConfiguration.Configure();
            EventEntityTypeConfiguration.Configure();
        }
    }
}
