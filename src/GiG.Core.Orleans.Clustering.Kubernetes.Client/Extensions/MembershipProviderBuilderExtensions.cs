using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using System;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Client.Extensions
{
    /// <summary>
    /// Membership Provider Builder Extensions.
    /// </summary>
    public static class MembershipProviderBuilderExtensions
    {
        private const string ProviderName = "Kubernetes";
        
        /// <summary>
        /// Configures Kubernetes in Orleans.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="IClientBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Kubernetes options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configuration));

            return builder;
        }

        /// <summary>
        /// Configures Kubernetes in Orleans.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="IClientBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Kubernetes options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureKubernetesClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            builder.RegisterProvider(ProviderName, x => x.ConfigureKubernetesClustering(configurationSection));

            return builder;
        }
    }
}
