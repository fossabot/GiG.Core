namespace GiG.Core.HealthChecks.Orleans.Abstractions
{
    /// <summary>
    /// Orleans HealthChecks Options.
    /// </summary>
    public class HealthCheckOptions
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
        /// Set to 'false' to disable the HealthCheck from self hosting.
        /// </summary>
        public bool HostSelf { get; set; } = true;
    }
}