using Microsoft.Extensions.DependencyInjection;
using POKA.Utils.Infrastructure.SqlServer.Repositories;

namespace POKA.Utils.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSqlServerInfrastructure(this IServiceCollection services)
        {
            // Sql Server
            services
                .AddScoped(typeof(IDbSetRepository<>), typeof(SqlServerDbSetRepository<>))
                .AddScoped<IEventStoreRepository, SqlEventStoreRepository>()
                .AddScoped<IRequestRepository, SqlServerRequestRepository>();

            return services;
        }
    }
}