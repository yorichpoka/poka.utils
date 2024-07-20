using Microsoft.Extensions.DependencyInjection;
using POKA.POC.WindowsService.Extensions;
using System.ServiceProcess;
using System;

namespace POKA.POC.WindowsService.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var serviceProvider = BuildServiceProvider();

            var servicesToRun = new ServiceBase[]
            {
                serviceProvider.GetRequiredService<MainService>()
            };

            ServiceBase.Run(servicesToRun);
        }

        public static IServiceProvider BuildServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
                                    .AddInfrastructure()
                                    .AddSingleton<MainService>()
                                    .BuildServiceProvider();

            return serviceProvider;
        }
    }
}
