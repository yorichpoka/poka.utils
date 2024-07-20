using POKA.POC.WindowsService.Extensions;
using System.ServiceProcess;
using Unity;

namespace POKA.POC.WindowsService.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var unityContainer = BuildUnityContainer();

            var servicesToRun = new ServiceBase[]
            {
                unityContainer.Resolve<MainService>()
            };

            ServiceBase.Run(servicesToRun);
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var unityContainer = new UnityContainer()
                                    .AddInfrastructure()
                                    .RegisterType<MainService>();

            return unityContainer;
        }
    }
}
