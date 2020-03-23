using OpenTelemetry.Trace.Configuration;

namespace GiG.Core.DistributedTracing.OpenTelemetry
{
    /// <summary>
    /// Tracing Exporter.
    /// </summary>
    public interface ITracingExporter
    {
        /// <summary>
        /// Register Tracing Exporter.
        /// </summary>
        void RegisterExporter(TracerBuilder builder);
    }
}