using System.Threading.Tasks;
using System.ServiceProcess;

namespace POKA.POC.WindowsService.WindowsService
{
    public class AppServiceBase : ServiceBase
    {
        public Task Start(params string[] args) => Task.Factory.StartNew(() => OnStart(args));

        public void Stop() => OnStop();
    }
}
