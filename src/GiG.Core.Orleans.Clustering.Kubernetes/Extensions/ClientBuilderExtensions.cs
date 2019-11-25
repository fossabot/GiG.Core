using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Clustering.Kubernetes;
using System;
using System.Configuration;

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
        /// <param name="clientBuilder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>THe <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder clientBuilder, IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return clientBuilder.ConfigureKubernetesClustering(configuration.GetSection(KubernetesOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="KubernetesOptions" /> will bind against.
        /// </summary>
        /// <param name="clientBuilder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder clientBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection}' is incorrect.");

            var kubernetesOptions = configurationSection.Get<KubernetesClientOptions>() ?? new KubernetesClientOptions();

            return
                clientBuilder.UseKubeGatewayListProvider(options =>
                {
                    options.Group = kubernetesOptions.Group;
                    options.CertificateData = kubernetesOptions.CertificateData;
                    options.APIEndpoint = kubernetesOptions.ApiEndpoint;
                    options.APIToken = kubernetesOptions.ApiToken;
                });
        }
    }
}