namespace GiG.Core.DistributedTracing.Telemetry.Abstractions
{
    /// <summary>
    /// Basic Exporter Options.
    /// </summary>
    public class BasicExporterOptions
    {
        /// <summary>
        /// A value to indicates whether a Exporter is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}