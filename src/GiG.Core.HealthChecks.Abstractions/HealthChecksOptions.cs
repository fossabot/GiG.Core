namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Health Checks Settings
    /// </summary>
    public class HealthChecksOptions
    {
        /// <summary>
        /// Health Checks default section name.
        /// </summary>
        public const string DefaultSectionName = "HealthChecks";

        /// <summary>
        /// The Url for the Live Health Check
        /// </summary>
        public string LiveUrl { get; set; } = "/health/live";

        /// <summary>
        /// The Url for the Ready Health Check
        /// </summary>
        public string ReadyUrl { get; set; } = "/health/ready";
    }
}
