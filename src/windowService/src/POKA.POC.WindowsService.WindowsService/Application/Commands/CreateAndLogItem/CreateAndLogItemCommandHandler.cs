using POKA.POC.WindowsService.WindowsService.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;

namespace POKA.POC.WindowsService.WindowsService.Application.Commands
{
    public class CreateAndLogItemCommandHandler : IRequestHandler<CreateAndLogItemCommand, Guid>
    {
        private readonly ILogger<CreateAndLogItemCommandHandler> _logger;

        public CreateAndLogItemCommandHandler(ILogger<CreateAndLogItemCommandHandler> logger)
        {
            _logger = logger;
        }

        public Task<Guid> Handle(CreateAndLogItemCommand request, CancellationToken cancellationToken)
        {
            var itemEntity = new ItemEntity(request.Value);

            this._logger.LogInformation(itemEntity.ToString());

            return Task.FromResult(itemEntity.Id);
        }
    }
}
