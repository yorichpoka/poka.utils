using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.Entities;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class EventEntityTypeConfiguration
    {
        public static void Configure()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(EventEntity)))
            {
                return;
            }

            BsonClassMap
                .RegisterClassMap<EventEntity>(
                    map => {
                        map.AutoMap();

                        map.SetIgnoreExtraElements(true);

                        map.MapProperty(l => l.AggregateId)
                           .SetSerializer(new BsonSerializerObjectId());
                    }
                );
        }
    }
}
