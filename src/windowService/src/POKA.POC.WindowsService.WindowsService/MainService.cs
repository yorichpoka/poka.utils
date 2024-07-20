using POKA.POC.WindowsService.WindowsService.Application.Commands;
using Microsoft.Extensions.Logging;
using System.ServiceProcess;
using System.Threading;
using MediatR;
using System;

namespace POKA.POC.WindowsService.WindowsService
{
    public partial class MainService : ServiceBase
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

        private void Working()
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
                var threadStrat = new ThreadStart(Working);
                this._workerThread = new Thread(threadStrat);
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
