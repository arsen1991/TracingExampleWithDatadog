using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tracing.Shared.Messages;

namespace Tracing.WebAapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleCommandController : ControllerBase
    {
        private readonly ILogger<SampleCommandController> _logger;
        readonly IBus _bus;
        private readonly IPublishEndpoint _publishEndpoint;


        public SampleCommandController(ILogger<SampleCommandController> logger, IBus bus, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _bus = bus;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {
            var activity = Activity.Current;

            var rc = await _bus.GetSendEndpoint(new Uri($"exchange:{OrderMessage.EndPointAddress}"));

            await rc.Send(new OrderMessage() { OrderId = 1, OrderCount = 2 });

            //await _publishEndpoint.Publish(new OrderMessage() { OrderId = 1, OrderCount = 2 });

            return Accepted();
        }
    }
}