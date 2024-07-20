using POKA.POC.WindowsService.WindowsService.Application.Interfaces;
using System.Configuration;

namespace POKA.POC.WindowsService.WindowsService.Infrastructure.Providers
{
    internal class AppSettingsProvider : IAppSettingsProvider
    {
        public string Variable1 => ConfigurationSettings.AppSettings.Get("Variable1");
    }
}
