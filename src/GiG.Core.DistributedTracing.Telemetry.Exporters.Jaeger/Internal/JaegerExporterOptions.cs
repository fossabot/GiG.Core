using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using GiG.Core.Hosting;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.Internal
{
    /// <summary>
    /// Jaeger Exporter Options.
    /// </summary>
    public class JaegerExporterOptions : BasicExporterOptions
    {
        /// <summary>
        /// Gets or sets the name of the service reporting telemetry.
        /// </summary>
        public string ServiceName { get; set; } = ApplicationMetadata.Name;

        /// <summary>
        /// Gets or sets the Jaeger agent host.
        /// </summary>
        public string AgentHost { get; set; } = "localhost";

        /// <summary>
        /// Gets or sets the Jaeger agent "compact thrift protocol" port.
        /// </summary
        public int AgentPort { get; set; } = 6831;

        /// <summary>
        /// Gets or sets the maximum packet size in bytes.
        /// </summary>
        public int? MaxPacketSize { get; set; } = 65000;

        /// <summary>
        /// Gets or sets the maximum time that should elapse between flushing the internal buffer to the configured Jaeger agent.
        /// </summary>
        public TimeSpan MaxFlushInterval { get; set; } = TimeSpan.FromSeconds(10);
    }
}