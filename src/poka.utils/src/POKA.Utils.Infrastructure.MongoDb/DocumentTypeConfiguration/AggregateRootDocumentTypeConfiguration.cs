using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public class AggregateRootDocumentTypeConfiguration : IDocumentTypeConfiguration
    {
        public void Configure()
        {
            BsonClassMap
                .TryRegisterClassMap<AggregateRoot<UserId>>(
                    map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                        map.MapProperty(l => l.CreatedByUserId)
                           .SetSerializer(new BsonSerializerObjectId<UserId>());
                    }
                );
        }
    }
}
