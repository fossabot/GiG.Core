using GiG.Core.Orleans.Clustering.Abstractions;
using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
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
        /// <param name="builder">The <see cref="IClientBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configuration));

            return builder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Client.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configurationSection));

            return builder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configuration));

            return builder;
        }

        /// <summary>
        /// Configures Kubernetes as a Membership Provider for an Orleans Silo.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configurationSection));

            return builder;
        }
    }
}