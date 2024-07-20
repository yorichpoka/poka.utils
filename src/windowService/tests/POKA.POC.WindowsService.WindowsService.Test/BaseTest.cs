using POKA.POC.WindowsService.Extensions;
using Unity;

namespace POKA.POC.WindowsService.WindowsService.Test
{
    public abstract class BaseTest
    {
        protected readonly IUnityContainer _unityContainer;

        protected BaseTest()
        {
            _unityContainer = new UnityContainer()
                                .AddInfrastructure();
        }
    }
}
