namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Ready HealthCheck tag.
        /// </summary>
        public const string ReadyTag = "ready";
        
        /// <summary>
        /// Live HealthCheck tag.
        /// </summary>
        public const string LiveTag = "live";

        /// <summary>
        /// Combined HealthCheck Url.
        /// </summary>
        public const string CombinedHealthCheckUrl = "/actuator/health";
    }
}