using System;

namespace GiG.Core.Orleans.Clustering.Abstractions
{
    /// <summary>
    /// Cluster Membership options.
    /// </summary>
    public class ClusterMembershipOptions
    {
        /// <summary>
        /// The period of time after which membership entries for defunct silos are eligible for removal.
        /// </summary>
        public TimeSpan DefunctSiloExpiration { get; set; } = TimeSpan.FromDays(3);

        
        /// <summary>
        /// The duration between membership table cleanup operations. This value is per-silo.
        /// </summary>
        public TimeSpan DefunctSiloCleanupPeriod { get; set; } = TimeSpan.FromDays(3);
    }
}