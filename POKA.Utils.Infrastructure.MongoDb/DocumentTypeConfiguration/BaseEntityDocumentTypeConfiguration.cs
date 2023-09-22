using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public class BaseEntityDocumentTypeConfiguration : IDocumentTypeConfiguration
    {
        public void Configure()
        {
            BsonClassMap
                .TryRegisterClassMap<BaseEntity<RequestId>>(
                    map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                        map.MapProperty(l => l.Id)
                           .SetElementName("_id")
                           .SetSerializer(new BsonSerializerObjectId<RequestId>());
                    }
                );

            BsonClassMap
                .TryRegisterClassMap<BaseEntity<EventId>>(
                    map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                        map.MapProperty(l => l.Id)
                           .SetElementName("_id")
                           .SetSerializer(new BsonSerializerObjectId<EventId>());
                    }
                );

            BsonClassMap
                .TryRegisterClassMap<BaseEntity<UserId>>(
                    map =>
                    {
                        map.AutoMap();
                        map.SetIgnoreExtraElements(true);
                        map.MapProperty(l => l.Id)
                           .SetElementName("_id")
                           .SetSerializer(new BsonSerializerObjectId<UserId>());
                    }
                );
        }
    }
}
