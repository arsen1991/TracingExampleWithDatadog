using MassTransit;
using Tracing.Shared.Messages;

namespace Tracing.WorkerService
{
    public class SubmitOrderConsumer : IConsumer<OrderMessage>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;
        private readonly IBus _bus;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }


        public async Task Consume(ConsumeContext<OrderMessage> context)
        {
            _logger.LogInformation("Request received.");

            await _bus.Publish(new OrderCompletedMessage() { OrderId = context.Message.OrderId });
        }
    }
}
