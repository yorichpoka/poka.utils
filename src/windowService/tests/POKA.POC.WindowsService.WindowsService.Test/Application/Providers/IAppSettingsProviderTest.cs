using POKA.POC.WindowsService.WindowsService.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;

namespace POKA.POC.WindowsService.WindowsService.Test.Application.Providers
{
    [TestClass]
    public class IAppSettingsProviderTest : BaseTest
    {
        [TestMethod]
        public void CanGetVariables()
        {
            // Arrange
            var appSettingsProvider = this._unityContainer.Resolve<IAppSettingsProvider>();

            // Act

            // Assert
            Assert.IsNotNull(appSettingsProvider);
            Assert.IsNotNull(appSettingsProvider.Variable1);
        }
    }
}
