using POKA.Utils.Infrastructure.MongoDb.DocumentTypeConfiguration;
using System.Reflection;

namespace POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration
{
    public static class MongoDBDocumentTypeConfiguration
    {
        public static void Configure()
        {
            var typeIDocumentTypeConfiguration = typeof(IDocumentTypeConfiguration);
            var documentTypeConfigurations = AppDomain
                                                .CurrentDomain
                                                .GetAssemblies()
                                                .SelectMany(l => l.GetTypes())
                                                .Where(l => l.IsClass)
                                                .Where(typeIDocumentTypeConfiguration.IsAssignableFrom)
                                                .Select(Activator.CreateInstance)
                                                .Select(l => l as IDocumentTypeConfiguration)
                                                .ToList();

            foreach (var documentTypeConfiguration in documentTypeConfigurations)
            {
                documentTypeConfiguration.Configure();
            }
        }
    }
}
