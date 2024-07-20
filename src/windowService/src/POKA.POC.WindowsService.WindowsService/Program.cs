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
            var unityContainer = BuildIocContainer();

            var servicesToRun = new ServiceBase[]
            {
                unityContainer.Resolve<MainService>()
            };

            ServiceBase.Run(servicesToRun);
        }

        public static IUnityContainer BuildIocContainer()
        {
            var unityContainer = new UnityContainer()
                                    .AddInfrastructure()
                                    .RegisterType<MainService>();

            return unityContainer;
        }
    }
}
