using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using MongoDB.Bson.Serialization.Conventions;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public class MongoDbConfiguration : IDocumentTypeConfiguration
    {
        public void Configure()
        {
            ConventionRegistry
                .Register(
                    "camelCase",
                    new ConventionPack {
                        new CamelCaseElementNameConvention()
                    },
                    l => true
                );
        }
    }
}
