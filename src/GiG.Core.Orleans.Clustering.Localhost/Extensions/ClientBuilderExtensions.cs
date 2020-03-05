using JetBrains.Annotations;
using Orleans;
using System;

namespace GiG.Core.Orleans.Clustering.Localhost.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Configures the client to connect to a Silo on the localhost.
        /// </summary>
        /// <param name="clientBuilder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="gatewayPort">The local silo's gateway port.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureLocalhostClustering([NotNull] this IClientBuilder clientBuilder, int gatewayPort = 30000, string serviceId = "dev", string clusterId = "dev")
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));

            return clientBuilder.UseLocalhostClustering(gatewayPort, serviceId, clusterId);
        }
    }
}