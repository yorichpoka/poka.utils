using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class CommonEntityTypeConfiguration
    {
        public static void Configure()
        {
            #region ChangeTracker

            if (BsonClassMap.IsClassMapRegistered(typeof(ChangeTracker)) == false)
            {
                BsonClassMap
                    .RegisterClassMap<ChangeTracker>(
                        map =>
                        {
                            map.UnmapMember(l => l.IsTracking);
                        }
                    );
            }

            #endregion

            #region BaseEntity

            if (
                BsonClassMap.IsClassMapRegistered(typeof(BaseEntity<RequestId>)) == false
            )
            {
                BsonClassMap
                    .RegisterClassMap<BaseEntity<RequestId>>(
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
                    .RegisterClassMap<BaseEntity<EventId>>(
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
                    .RegisterClassMap<BaseEntity<UserId>>(
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

            #endregion

            #region BaseAggregate

            if (BsonClassMap.IsClassMapRegistered(typeof(AggregateRoot<UserId>)) == false)
            {
                BsonClassMap
                    .RegisterClassMap<AggregateRoot<UserId>>(
                        map => {
                            map.AutoMap();
                            map.SetIgnoreExtraElements(true);
                            map.MapProperty(l => l.CreatedByUserId)
                               .SetSerializer(new BsonSerializerObjectId<UserId>());
                        }
                    );

            }

            #endregion
        }
    }
}
