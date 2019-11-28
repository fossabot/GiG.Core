namespace GiG.Core.HealthChecks.Orleans.Abstractions
{
    /// <summary>
    /// Orleans HealthChecks Options.
    /// </summary>
    public class HealthChecksOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:HealthChecks";

        /// <summary>
        /// The Domain(s) on which Kestrel will listen.
        /// </summary>
        public string DomainFilter { get; set; } = "*";

        /// <summary>
        /// The Port on which the Kestrel will listen.
        /// </summary>
        public int Port { get; set; } = 5555;

        /// <summary>
        /// The Url for the HealthCheck.
        /// </summary>
        public string HealthCheckUrl { get; set; } = "/health";

    }
}