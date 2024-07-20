﻿using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using POKA.Utils.Infrastructure.MongoDb.Serializers;
using MongoDB.Bson.Serialization;
using POKA.Utils.ValueObjects;
using POKA.Utils.Entities;
using POKA.Utils.Enums;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public class RequestEntityDocumentTypeConfiguration : IDocumentTypeConfiguration
    {
        public void Configure()
        {
            BsonClassMap
                .TryRegisterClassMap<RequestEntity>(
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

                        map.MapProperty(l => l.CreatedByUserId)
                           .SetSerializer(new BsonSerializerObjectId<UserId>());
                    }
                );
        }
    }
}
