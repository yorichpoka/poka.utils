using MongoDB.Bson.Serialization.Conventions;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class MongoDbConfiguration
    {
        public static void Configure()
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
