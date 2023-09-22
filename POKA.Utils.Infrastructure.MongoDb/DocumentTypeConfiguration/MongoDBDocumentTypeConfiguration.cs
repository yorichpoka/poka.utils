namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class MongoDBDocumentTypeConfiguration
    {
        public static void Configure()
        {
            MongoDbConfiguration.Configure();
            RequestEntityDocumentTypeConfiguration.Configure();
            CommonDocumentTypeConfiguration.Configure();
            EventEntityDocumentTypeConfiguration.Configure();
        }
    }
}
