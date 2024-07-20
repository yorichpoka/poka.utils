using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using MongoDB.Bson.Serialization;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public class ChangeTrackerDocumentTypeConfiguration : IDocumentTypeConfiguration
    {
        public void Configure()
        {
            BsonClassMap
                .TryRegisterClassMap<ChangeTracker>(
                    map =>
                    {
                        map.UnmapMember(l => l.IsTracking);
                    }
                );
        }
    }
}
