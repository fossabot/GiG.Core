using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Clustering.Kubernetes;
using System;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="KubernetesOptions" /> will bind against.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>THe <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder builder, IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.ConfigureKubernetesClustering(configuration.GetSection(KubernetesOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="KubernetesOptions" /> will bind against.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var kubernetesOptions = configurationSection.Get<KubernetesClientOptions>() ?? new KubernetesClientOptions();

            return
                builder.UseKubeGatewayListProvider(options =>
                {
                    options.Group = kubernetesOptions.Group;
                    options.CertificateData = kubernetesOptions.CertificateData;
                    options.APIEndpoint = kubernetesOptions.ApiEndpoint;
                    options.APIToken = kubernetesOptions.ApiToken;
                });
        }
    }
}
