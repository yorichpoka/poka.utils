using POKA.POC.WindowsService.WindowsService.Infrastructure.Providers;
using POKA.POC.WindowsService.WindowsService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Unity.Microsoft.DependencyInjection;
using System.Reflection;
using Unity.Lifetime;
using Unity;

namespace POKA.POC.WindowsService.Extensions
{
    public static class DependencyInjection
    {
        public static IUnityContainer AddInfrastructure(this IUnityContainer unityContainer)
        {
            unityContainer
                .RegisterType<IAppSettingsProvider, AppSettingsProvider>(new SingletonLifetimeManager())
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
    }
}
