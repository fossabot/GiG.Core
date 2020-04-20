using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using GiG.Core.Hosting;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Internal
{
    /// <summary>
    /// Zipkin Exporter Options.
    /// </summary>
    public class ZipkinExporterOptions : BasicExporterOptions
    {
        /// <summary>
        /// Gets or sets the name of the service reporting telemetry.
        /// </summary>
        public string ServiceName { get; set; } = ApplicationMetadata.Name;

        /// <summary>
        /// Gets or sets Zipkin endpoint address.
        /// </summary>
        public Uri Endpoint { get; set; } = new Uri("http://localhost:9411/api/v2/spans");

        /// <summary>
        /// Gets or sets timeout in seconds.
        /// </summary>
        public TimeSpan TimeoutSeconds { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary>
        /// Gets or sets value indicating whether short trace id should be used.
        /// </summary>
        public bool UseShortTraceIds { get; set; } = false;
    }
}