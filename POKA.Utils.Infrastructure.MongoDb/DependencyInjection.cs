using POKA.Utils.Infrastructure.MongoDb.EntityTypeConfiguration;
using POKA.Utils.Infrastructure.MongoDb.Repositories;
using POKA.Utils.Infrastructure.MongoDb.DbContexts;
using Microsoft.Extensions.DependencyInjection;
using POKA.Utils.Repositories;
using MongoDB.Driver;

namespace POKA.Utils.Infrastructure.MongoDb
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMongoDbInfrastructure(this IServiceCollection services)
        {
            MongoDBEntityTypeConfiguration.Configure();

            // Singleton
            services
                .AddSingleton<EventStoreMongoDatabase>(
                    serviceProvider =>
                    {
                        var appSettingsProvider = serviceProvider.GetRequiredService<IMongoDbConnectionStringsProvider>();
                        var mongoClient = new MongoClient(appSettingsProvider.EventStore.connectionString);
                        var mongoDataBase = mongoClient.GetDatabase(appSettingsProvider.EventStore.dataBaseName);
                        var dataBase = new EventStoreMongoDatabase(mongoDataBase, appSettingsProvider.AllowUsingOfTransaction);

                        return dataBase;
                    }
                )
                .AddSingleton<MasterMongoDatabase>(
                    serviceProvider =>
                    {
                        var appSettingsProvider = serviceProvider.GetRequiredService<IMongoDbConnectionStringsProvider>();
                        var mongoClient = new MongoClient(appSettingsProvider.Master.connectionString);
                        var mongoDataBase = mongoClient.GetDatabase(appSettingsProvider.Master.dataBaseName);
                        var dataBase = new MasterMongoDatabase(mongoClient, mongoDataBase, appSettingsProvider.AllowUsingOfTransaction);

                        return dataBase;
                    }
                );

            // Sql Server
            services
                .AddScoped(typeof(IDbSetRepository<>), typeof(MongoDbServerRepository<>))
                .AddScoped<IEventStoreRepository, MongoDbEventStoreRepository>()
                .AddScoped<IRequestRepository, MongoDbRequestRepository>();

            return services;
        }
    }
}