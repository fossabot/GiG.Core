namespace GiG.Core.Orleans.Silo.Abstractions
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
        /// Set the dashboard to host it's own http server (default is true)
        /// </summary>
        public bool HostSelf { get; set; } = true;
    }
}