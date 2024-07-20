using POKA.POC.WindowsService.WindowsService.Application.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;

namespace POKA.POC.WindowsService.WindowsService.Test.Application.Commands
{
    [TestClass]
    public class CreateAndLogItemCommandTest : BaseTest
    {
        [TestMethod]
        public void CanExecuteCommand()
        {
            // Arrange
            var mediator = this._ServiceProvider.GetRequiredService<IMediator>();
            var createAndLogItemCommand = new CreateAndLogItemCommand(DateTime.UtcNow.ToString());

            // Act
            var result = mediator.Send(createAndLogItemCommand);

            // Assert
            Assert.AreNotEqual(result, Guid.Empty);
        }
    }
}
