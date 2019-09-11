namespace GiG.Core.Orleans.Abstractions.Configuration
{
    /// <summary>
    /// Orleans Consul Settings
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string DefaultConfigurationSection = "Orleans:Consul";
      
        /// <summary>
        /// Consul Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// KV Root Folder
        /// </summary>
        public string KvRootFolder { get; set; }
    }
}
