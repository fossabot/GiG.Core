namespace GiG.Core.Orleans.Clustering.Consul.Silo.Configurations
{
    /// <summary>
    /// Orleans Silo Consul Settings.
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Consul";

        /// <summary>
        /// Consul Address.
        /// </summary>
        public string Address { get; set; } = "http://localhost:8500";

        /// <summary>
        /// Key Value Root Folder.
        /// </summary>
        public string KvRootFolder { get; set; } = "dev";
    }
}
