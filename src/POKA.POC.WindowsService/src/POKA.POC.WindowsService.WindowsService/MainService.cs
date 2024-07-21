using POKA.POC.WindowsService.WindowsService.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Threading;
using System;

namespace POKA.POC.WindowsService.WindowsService
{
    [RunInstaller(true)]
    public partial class MainService : AppServiceBase
    {
        private readonly ILogger<MainService> _logger;
        private Thread _workerThread;

        public MainService(ILogger<MainService> logger)
        {
            InitializeComponent();

            _logger = logger;
        }

        private void Work()
        {
            while (true)
            {
                var itemEntity = new ItemEntity(DateTime.UtcNow.ToString());

                this._logger.LogInformation(itemEntity.ToString());

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
