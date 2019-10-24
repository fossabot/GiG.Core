using GiG.Core.Orleans.Clustering.Abstractions;

namespace GiG.Core.Orleans.Clustering.Consul.Abstractions
{
    /// <summary>
    /// Consul Options.
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = MembershipProviderOptions.DefaultSectionName;

        /// <summary>
        /// The Consul Address.
        /// </summary>
        public string Address { get; set; } = "http://localhost:8500";

        /// <summary>
        /// The Key Value Root Folder.
        /// </summary>
        public string KvRootFolder { get; set; } = "dev";
    }
}