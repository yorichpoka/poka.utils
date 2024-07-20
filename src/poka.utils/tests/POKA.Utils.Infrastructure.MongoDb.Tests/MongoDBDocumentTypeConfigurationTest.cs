using POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration;

namespace POKA.Utils.Infrastructure.MongoDb.Tests
{
    public class MongoDBDocumentTypeConfigurationTest
    {
        [Fact]
        public void CanConfigure()
        {
            MongoDBDocumentTypeConfiguration.Configure();
        }
    }
}