namespace GiG.Core.Orleans.Abstractions.Configuration
{
    /// <summary>
    /// Orleans Dashboard Settings
    /// </summary>
    public class DashboardOptions
    {
        public const string DefaultConfigurationSection = "Orleans:Dashboard";

        /// <summary>
        /// Determines whether the Dashboard should be configured.
        /// </summary>
        public bool Enabled { get; set; }
        
        /// <summary>
        /// Sets the Dashboard port. Default is 8181.
        /// </summary>
        public int Port { get; set; } = 8181;

        /// <summary>
        /// Sets the Dashboard path.
        /// </summary>
        public string Path { get; set; }
    }
}