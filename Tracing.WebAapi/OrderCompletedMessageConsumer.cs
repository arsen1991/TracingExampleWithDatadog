using MassTransit;
using Tracing.Shared.Messages;

namespace Tracing.WebAapi
{
    public class OrderCompletedMessageConsumer : IConsumer<OrderCompletedMessage>
    {
        private readonly ILogger<OrderCompletedMessageConsumer> _logger;
        public OrderCompletedMessageConsumer(ILogger<OrderCompletedMessageConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OrderCompletedMessage> context)
        {
            _logger.LogInformation("Order completed message received.");
            return Task.CompletedTask;
        }
    }
}
