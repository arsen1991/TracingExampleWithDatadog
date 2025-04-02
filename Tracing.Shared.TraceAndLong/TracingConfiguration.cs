using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Tracing.Shared.TraceAndLong
{
    internal static class Tracing
    {
        internal static void AddTracing(this IServiceCollection services, TracingAndLoggingConfiguration configuration)
        {
            services.AddOpenTelemetryTracing((builder) => builder
                .AddInstrumentations(configuration)
                .AddSource(configuration.ServiceName)
                .SetResourceBuilder(
                    ResourceBuilder.CreateDefault().
                    AddService(serviceName: configuration.ServiceName, serviceVersion: configuration.ServiceVersion))
                .AddOtlpExporter());
        }

        private static TracerProviderBuilder AddInstrumentations(this TracerProviderBuilder builder, TracingAndLoggingConfiguration tracingAndLoggingConfiguration)
        {
            if (tracingAndLoggingConfiguration.Instrumentations.ASPNETCORE)
            {
                builder.AddAspNetCoreInstrumentation();
            }

            if (tracingAndLoggingConfiguration.Instrumentations.MassTransit)
            {
                builder.AddSource("MassTransit");
            }

            return builder;
        }
    }
}
