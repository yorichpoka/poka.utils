using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;
using POKA.Utils.Entities;
using POKA.Utils.Enums;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class RequestEntityTypeConfiguration
    {
        public static void Configure()
        {
            if (BsonClassMap.IsClassMapRegistered(typeof(RequestEntity)))
            {
                return;
            }

            BsonClassMap
                .RegisterClassMap<RequestEntity>(
                    map => {
                        map.AutoMap();

                        map.SetIgnoreExtraElements(true);

                        map.MapProperty(l => l.Status)
                           .SetSerializer(new BsonSerializerSmartEnum<RequestStatusEnum>());

                        map.MapProperty(l => l.Type)
                           .SetSerializer(new BsonSerializerSmartEnum<RequestTypeEnum>());

                        map.MapProperty(l => l.ScopeId)
                           .SetSerializer(new BsonSerializerObjectId<RequestScopeId>());

                        map.MapProperty(l => l.ParentId)
                           .SetSerializer(new BsonSerializerObjectId<RequestId>());
                    }
                );
        }
    }
}
