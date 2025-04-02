using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace Tracing.Shared.TraceAndLong
{
    internal static class Logging
    {
        internal static void AddLogging(this ILoggingBuilder loggingBuilder, TracingAndLoggingConfiguration configuration)
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;

                options.SetResourceBuilder(ResourceBuilder.CreateDefault().
                    AddService(serviceName: configuration.ServiceName, serviceVersion: configuration.ServiceVersion));

                options.AddConsoleExporter();
            });
        }
    }
}
