using OpenTelemetry.Exporter;

namespace Tracing.Shared.TraceAndLong
{
    internal class TracingAndLoggingConfiguration
    {
        public string ServiceName { get; set; }

        public string ServiceVersion { get; set; }

        public Instrumentations Instrumentations { get; set; }
        public OtlpExportProtocol Protocol { get; set; }
        public string EndPoint { get; set; }
    }

    internal class Instrumentations
    {
        public bool ASPNETCORE { get; set; }

        public bool MassTransit { get; set; }
    }
}
