using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Extensions
{
    /// <summary>
    /// Membership Provider Builder Extensions.
    /// </summary>
    public static class MembershipProviderBuilderExtensions
    {
        private const string ProviderName = "Kubernetes";

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Client.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> clientBuilder, [NotNull] IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            clientBuilder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configuration));

            return clientBuilder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Client.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> clientBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection}' is incorrect.");

            clientBuilder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configurationSection));

            return clientBuilder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            siloBuilder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configuration));

            return siloBuilder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> siloBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection}' is incorrect.");

            siloBuilder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configurationSection));

            return siloBuilder;
        }
    }
}