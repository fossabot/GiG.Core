namespace GiG.Core.Orleans.Silo.Dashboard.Abstractions
{
    /// <summary>
    /// Orleans Silo Dashboard Options.
    /// </summary>
    public class DashboardOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Dashboard";

        /// <summary>
        /// A value to indicate if the Dashboard is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// The Dashboard port.
        /// </summary>
        public int Port { get; set; } = 8080;

        /// <summary>
        /// The Dashboard Url.
        /// </summary>
        public string Path { get; set; } = "/dashboard";

        /// <summary>
        /// Set the dashboard to host it's own HTTP server.
        /// </summary>
        public bool HostSelf { get; set; } = false;

        /// <summary>
        /// Set the username for Basic AUTH.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Set the password for Basic AUTH.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Set the number of milliseconds between counter samples.
        /// </summary>
        public int CounterUpdateIntervalMs { get; set; } = 10_000;

        /// <summary>
        /// Value to indicate if the trace functionality is disabled or not.
        /// </summary>
        public bool HideTrace { get; set; }
    }
}