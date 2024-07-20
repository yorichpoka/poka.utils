using POKA.POC.WindowsService.WindowsService.Infrastructure.Providers;
using POKA.POC.WindowsService.WindowsService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Unity.Microsoft.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using System.Reflection;
using Unity.Injection;
using Unity.Lifetime;
using Serilog;
using System;
using Unity;

namespace POKA.POC.WindowsService.Extensions
{
    public static class DependencyInjection
    {
        public static IUnityContainer AddInfrastructure(this IUnityContainer unityContainer)
        {
            unityContainer
                .RegisterType<IAppSettingsProvider, AppSettingsProvider>(new SingletonLifetimeManager())
                .AddLogger()
                .AddMediatR();

            return unityContainer;
        }

        private static IUnityContainer AddMediatR(this IUnityContainer unityContainer)
        {
            var serviceCollection = new ServiceCollection()
                                        .AddMediatR(
                                            mediatRServiceConfiguration =>
                                            {
                                                mediatRServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                                            }
                                        );

            unityContainer.BuildServiceProvider(serviceCollection);

            return unityContainer;
        }

        private static IUnityContainer AddLogger(this IUnityContainer unityContainer)
        {
            Log.Logger = new LoggerConfiguration()
                            .WriteTo
                                .File(
                                    path: $"logs/{DateTime.UtcNow:dd-MM-yyyy}.json"
                                )
                            .CreateLogger();

            unityContainer
                .RegisterInstance<ILoggerFactory>(new SerilogLoggerFactory(), new SingletonLifetimeManager())
                .RegisterInstance(
                    t: typeof(ILogger<>), 
                    instance: new InjectionFactory(
                        (_unityContainer, type, name) =>
                        {
                            var loggerFactory = _unityContainer.Resolve<ILoggerFactory>();
                            var loggerType = type.GenericTypeArguments[0];
                            var logger = loggerFactory.CreateLogger(loggerType);

                            return logger;
                        }
                    ),
                    lifetimeManager: new SingletonLifetimeManager()
                );

            return unityContainer;
        }
    }
}
