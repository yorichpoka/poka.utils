using System.ServiceProcess;

namespace POKA.POC.WindowsService.WindowsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new MainService()
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
