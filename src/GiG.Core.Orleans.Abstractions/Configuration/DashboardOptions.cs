namespace GiG.Core.Orleans.Abstractions.Configuration
{
    /// <summary>
    /// Orleans Dashboard Settings.
    /// </summary>
    public class DashboardOptions
    {
        /// <summary>
        /// Default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Dashboard";

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