using JetBrains.Annotations;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Localhost.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configures the silo to use development-only clustering and listen on localhost.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="siloPort">The silo port.</param>
        /// <param name="gatewayPort">The gateway port.</param>
        /// <param name="primarySiloEndpoint">
        /// The endpoint of the primary silo, or <see langword="null"/> to use this silo as the primary.
        /// </param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder ConfigureLocalhostClustering([NotNull] this ISiloBuilder siloBuilder, int siloPort = 11111, int gatewayPort = 30000, System.Net.IPEndPoint primarySiloEndpoint = null, string serviceId = "dev", string clusterId = "dev")
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
                 
            return  siloBuilder.UseLocalhostClustering(siloPort, gatewayPort, primarySiloEndpoint, serviceId, clusterId);
        }
    }
}
