namespace GiG.Core.Metrics.Prometheus.Orleans.Silo.Abstractions
{
    /// <summary>
    /// The Orleans Metrics Options.
    /// </summary>
    public class OrleansMetricsOptions
    {
        /// <summary>
        /// The default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Metrics";

        /// <summary>
        /// A value to indicate if the Orleans Metrics is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}