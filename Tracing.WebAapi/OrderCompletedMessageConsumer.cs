using Datadog.Trace;
using MassTransit;
using Tracing.Shared.Messages;
using static Tracing.Shared.Messages.HeaderParser;

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
            var spanContextExtractor = new SpanContextExtractor();
            var parentContext = spanContextExtractor.Extract(context.Headers.ToDictionary(x => x.Key, y => y.Value), (headers, key) => GetHeaderValues(headers, key));
            var spanCreationSettings = new SpanCreationSettings() { Parent = parentContext };
            using var scope = Tracer.Instance.StartActive("operation", spanCreationSettings);

            _logger.LogInformation("Order completed message received.");
            return Task.CompletedTask;
        }
    }
}
