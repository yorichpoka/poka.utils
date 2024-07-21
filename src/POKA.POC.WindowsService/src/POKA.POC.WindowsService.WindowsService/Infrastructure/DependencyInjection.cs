using POKA.POC.WindowsService.WindowsService.Infrastructure.Providers;
using POKA.POC.WindowsService.WindowsService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Formatting.Json;
using Serilog;
using System;

namespace POKA.POC.WindowsService.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IAppSettingsProvider, AppSettingsProvider>()
                .AddLogger();

            return serviceCollection;
        }

        private static IServiceCollection AddLogger(this IServiceCollection serviceCollection)
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo
                                .File(
                                    path: $"C:\\logs\\{DateTime.UtcNow:dd-MM-yyyy}.json",
                                    formatter: new JsonFormatter()
                                )
                            .CreateLogger();

            serviceCollection
                .AddLogging(
                    l =>
                    {
                        l.AddSerilog(dispose: true);
                    }
                );

            return serviceCollection;
        }
    }
}
