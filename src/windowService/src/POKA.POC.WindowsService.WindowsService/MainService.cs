using System.ServiceProcess;
using MediatR;

namespace POKA.POC.WindowsService.WindowsService
{
    public partial class MainService : ServiceBase
    {
        private readonly IMediator _mediator;

        public MainService(IMediator mediator)
        {
            InitializeComponent();

            _mediator = mediator;
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
