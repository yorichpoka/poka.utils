using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;

namespace POKA.POC.WindowsService.WindowsService.Test
{
    [TestClass]
    public class MainServiceTest : BaseTest
    {
        [TestMethod]
        public void CanStart()
        {
            // Arrange
            var mainService = this._ServiceProvider.GetRequiredService<MainService>();

            // Act
            Task.Factory.StartNew(() => mainService.Start());
            Thread.Sleep(10000);
            mainService.Stop();

            // Assert
            Assert.IsNotNull(mainService);
        }
    }
}
