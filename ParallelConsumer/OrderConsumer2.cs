using Datadog.Trace;
using MassTransit;
using Tracing.Shared.Messages;
using static Tracing.Shared.Messages.HeaderParser;

namespace ParallelConsumer
{
    public class OrderConsumer2 : IConsumer<OrderMessage>
    {
        private readonly ILogger<OrderConsumer2> _logger;
        private readonly IBus _bus;
        public OrderConsumer2(ILogger<OrderConsumer2> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        public Task Consume(ConsumeContext<OrderMessage> context)
        {
            var spanContextExtractor = new SpanContextExtractor();
            var parentContext = spanContextExtractor.Extract(context.Headers.ToDictionary(x => x.Key, y => y.Value), (headers, key) => GetHeaderValues(headers, key));
            var spanCreationSettings = new SpanCreationSettings() { Parent = parentContext };
            using var scope = Tracer.Instance.StartActive("operation", spanCreationSettings);

            _logger.LogInformation("Request received.");
            return Task.CompletedTask;
        }
    }
}
