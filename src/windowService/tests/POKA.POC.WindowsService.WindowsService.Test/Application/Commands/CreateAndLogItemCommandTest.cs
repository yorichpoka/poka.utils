using POKA.POC.WindowsService.WindowsService.Application.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MediatR;
using System;
using Unity;

namespace POKA.POC.WindowsService.WindowsService.Test.Application.Commands
{
    [TestClass]
    public class CreateAndLogItemCommandTest : BaseTest
    {
        [TestMethod]
        public void CanExecuteCommand()
        {
            // Arrange
            var mediator = this._unityContainer.Resolve<IMediator>();
            var createAndLogItemCommand = new CreateAndLogItemCommand(DateTime.UtcNow.ToString());

            // Act
            var result = mediator.Send(createAndLogItemCommand);

            // Assert
            Assert.AreNotEqual(result, Guid.Empty);
        }
    }
}
