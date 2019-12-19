namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Health Checks Options.
    /// </summary>
    public class HealthChecksOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "HealthChecks";

        /// <summary>
        /// The Url for the Ready + Live Health Checks.
        /// </summary>
        public string CombinedUrl { get; set; } = Constants.CombinedHealthCheckUrl;

        /// <summary>
        /// The Url for the Live HealthCheck.
        /// </summary>
        public string LiveUrl { get; set; }

        /// <summary>
        /// The Url for the Ready HealthCheck.
        /// </summary>
        public string ReadyUrl { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public HealthChecksOptions()
        {
            LiveUrl = $"{Constants.CombinedHealthCheckUrl} / {Constants.LiveTag}";
            ReadyUrl = $"{Constants.CombinedHealthCheckUrl} / {Constants.ReadyTag}";
        }
    }
}