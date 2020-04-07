using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Localhost.Extensions
{
    /// <summary>
    /// Membership Provider Builder Extensions.
    /// </summary>
    public static class MembershipProviderBuilderExtensions
    {
        private const string ProviderName = "Localhost";

        /// <summary>
        /// Configures Localhost as a Membership Provider for an Orleans Client.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder" />.</param>
        /// <param name="gatewayPort">The local silo's gateway port.</param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>The<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureLocalhostClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> clientBuilder, int gatewayPort = 30000, string serviceId = "dev", string clusterId = "dev")
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));

            clientBuilder.RegisterProvider(ProviderName, x => x.UseLocalhostClustering(gatewayPort, serviceId, clusterId));

            return clientBuilder;
        }

        /// <summary>
        /// Configures Localhost as a Membership Provider for an Orleans Client.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureLocalhostClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> clientBuilder, [NotNull] IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var clusterConfigurationSection = configuration.GetSection(Constants.ClusterOptionsDefaultSectionName);
            var clusterOptions = clusterConfigurationSection.Get<ClusterOptions>() ?? new ClusterOptions();

            var endpointConfigurationSection = configuration.GetSection(Constants.EndpointOptionsDefaultSectionName);
            var endpointOptions = endpointConfigurationSection?.Get<EndpointOptions>() ?? new EndpointOptions();

            return clientBuilder.ConfigureLocalhostClustering(endpointOptions.GatewayPort, clusterOptions.ServiceId, clusterOptions.ClusterId);
        }

        /// <summary>
        /// Configures Localhost as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="siloPort">The silo port.</param>
        /// <param name="gatewayPort">The gateway port.</param>
        /// <param name="primarySiloEndpoint">
        /// The endpoint of the primary silo, or <see langword="null"/> to use this silo as the primary.
        /// </param>
        /// <param name="serviceId">The service id.</param>
        /// <param name="clusterId">The cluster id.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureLocalhostClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> siloBuilder, int siloPort = 11111, int gatewayPort = 30000, System.Net.IPEndPoint primarySiloEndpoint = null, string serviceId = "dev", string clusterId = "dev")
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));

            siloBuilder.RegisterProvider(ProviderName, x => x.UseLocalhostClustering(siloPort, gatewayPort, primarySiloEndpoint, serviceId, clusterId));

            return siloBuilder;
        }

        /// <summary>
        /// Configures Localhost as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureLocalhostClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> siloBuilder, IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var clusterConfigurationSection = configuration.GetSection(Constants.ClusterOptionsDefaultSectionName);
            var clusterOptions = clusterConfigurationSection.Get<ClusterOptions>() ?? new ClusterOptions();

            var endpointConfigurationSection = configuration.GetSection(Constants.EndpointOptionsDefaultSectionName);
            var endpointOptions = endpointConfigurationSection?.Get<EndpointOptions>() ?? new EndpointOptions();

            return siloBuilder.ConfigureLocalhostClustering(endpointOptions.SiloPort, endpointOptions.GatewayPort, null, clusterOptions.ServiceId, clusterOptions.ClusterId);
        }
    }
}