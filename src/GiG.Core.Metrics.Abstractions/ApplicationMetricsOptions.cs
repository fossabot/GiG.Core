namespace GiG.Core.Metrics.Abstractions
{
    /// <summary>
    /// The Application Metrics Options.
    /// </summary>
    public class ApplicationMetricsOptions
    {
        /// <summary>
        /// The default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "ApplicationMetrics";
        
        /// <summary>
        /// The Url for the Application Metrics.
        /// </summary>
        public string Url { get; set; } = "/metrics";

        /// <summary>
        /// A value to indicate if the Application Metrics are enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}