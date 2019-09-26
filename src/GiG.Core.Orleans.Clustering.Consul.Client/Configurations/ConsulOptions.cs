namespace GiG.Core.Orleans.Clustering.Consul.Client.Configurations
{
    /// <summary>
    /// Orleans Client Consul Settings.
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string DefaultSectionName = MembershipProviderOptions.DefaultSectionName;

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
