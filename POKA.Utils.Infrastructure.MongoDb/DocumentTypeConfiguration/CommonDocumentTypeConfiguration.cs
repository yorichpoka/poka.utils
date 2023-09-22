using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class CommonDocumentTypeConfiguration
    {
        public static void Configure()
        {
            #region ChangeTracker

            BsonClassMap
                .TryRegisterClassMap<ChangeTracker>(
                    map =>
                    {
                        map.UnmapMember(l => l.IsTracking);
                    }
                );

            #endregion

            #region BaseEntity

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

            #endregion

            #region BaseAggregate

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

            #endregion
        }
    }
}
