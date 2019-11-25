using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Telemetry.Abstractions
{
    /// <summary>
    /// Tracing Options.
    /// </summary>
    public class TracingOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Tracing";

        /// <summary>
        /// A value to indicates whether a Exporter is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; }
        
        /// <summary>
        /// The Tracing Exporters.
        /// </summary>
        public IDictionary<string, BasicExporterOptions> Exporters { get; set; } = new Dictionary<string, BasicExporterOptions>();
    }
}