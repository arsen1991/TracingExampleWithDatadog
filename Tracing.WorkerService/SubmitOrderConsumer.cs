﻿using Datadog.Trace;
using MassTransit;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using RabbitMQ.Client;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using Tracing.Shared.Messages;
using static Tracing.Shared.Messages.HeaderParser;

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
            var spanContextExtractor = new SpanContextExtractor();
            var parentContext = spanContextExtractor.Extract(context.Headers.ToDictionary(x => x.Key, y => y.Value), (headers, key) => GetHeaderValues(headers, key));
            var spanCreationSettings = new SpanCreationSettings() { Parent = parentContext };
            using var scope = Tracer.Instance.StartActive("operation", spanCreationSettings);

            _logger.LogInformation("Request received.");

            await _bus.Publish(new OrderCompletedMessage() { OrderId = context.Message.OrderId });
        }
    }
}
