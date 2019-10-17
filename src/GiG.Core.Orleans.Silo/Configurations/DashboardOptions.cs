namespace GiG.Core.Orleans.Silo.Configurations
{
    /// <summary>
    /// Orleans Silo Dashboard Settings.
    /// </summary>
    public class DashboardOptions
    {
        /// <summary>
        /// Default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Dashboard";

        /// <summary>
        /// Determines whether the Dashboard is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = false;
        
        /// <summary>
        /// Sets the Dashboard port.
        /// </summary>
        public int Port { get; set; } = 8080;

        /// <summary>
        /// Sets the Dashboard path.
        /// </summary>
        public string Path { get; set; } = "/dashboard";
    }
}