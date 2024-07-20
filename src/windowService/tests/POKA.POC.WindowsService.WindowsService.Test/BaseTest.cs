using Microsoft.Extensions.DependencyInjection;
using POKA.POC.WindowsService.Extensions;
using System;

namespace POKA.POC.WindowsService.WindowsService.Test
{
    public abstract class BaseTest
    {
        protected readonly IServiceProvider _ServiceProvider;

        protected BaseTest()
        {
            _ServiceProvider =  new ServiceCollection()
                                    .AddInfrastructure()
                                    .AddSingleton<MainService>()
                                    .BuildServiceProvider();
        }
    }
}
