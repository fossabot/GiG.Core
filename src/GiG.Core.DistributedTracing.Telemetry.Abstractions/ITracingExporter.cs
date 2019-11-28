namespace GiG.Core.DistributedTracing.Telemetry.Abstractions
{
    /// <summary>
    /// Tracing Exporter.
    /// </summary>
    public interface ITracingExporter
    {
        /// <summary>
        /// Register Tracing Exporter.
        /// </summary>
        void RegisterExporter(string exporter);
    }
}