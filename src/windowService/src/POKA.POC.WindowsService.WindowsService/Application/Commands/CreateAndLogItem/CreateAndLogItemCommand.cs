using MediatR;
using System;

namespace POKA.POC.WindowsService.WindowsService.Application.Commands
{
    public class CreateAndLogItemCommand : IRequest<Guid>
    {
        public string Value { get; private set; }

        public CreateAndLogItemCommand()
        {
        }

        public CreateAndLogItemCommand(string value)
        {
            Value = value;
        }
    }
}
