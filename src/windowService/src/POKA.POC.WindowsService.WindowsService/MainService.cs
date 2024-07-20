using POKA.POC.WindowsService.WindowsService.Application.Commands;
using Microsoft.Extensions.Logging;
using System.ServiceProcess;
using System.ComponentModel;
using System.Threading;
using MediatR;
using System;

namespace POKA.POC.WindowsService.WindowsService
{
    [RunInstaller(true)]
    public partial class MainService : AppServiceBase
    {
        private readonly ILogger<MainService> _logger;
        private readonly IMediator _mediator;
        private Thread _workerThread;

        public MainService(IMediator mediator, ILogger<MainService> logger)
        {
            InitializeComponent();

            _mediator = mediator;
            _logger = logger;
        }

        private void Work()
        {
            while (true)
            {
                var createAndLogItemCommand = new CreateAndLogItemCommand(DateTime.UtcNow.ToString());

                this._mediator
                    .Send(createAndLogItemCommand)
                    .Wait();

                Thread.Sleep(3000);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                var threadStart = new ThreadStart(Work);
                this._workerThread = new Thread(threadStart);
                this._workerThread.Start();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);

                throw;
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (this._workerThread?.IsAlive ?? false)
                {
                    this._workerThread.Abort();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, ex.Message);

                throw;
            }
        }
    }
}
