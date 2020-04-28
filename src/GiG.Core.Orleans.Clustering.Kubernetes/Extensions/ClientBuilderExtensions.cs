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
    /// The <see cref="IClientBuilder" /> Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Configures Kubernetes Clustering.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder builder, IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.ConfigureKubernetesClustering(configuration.GetSection(KubernetesOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures Kubernetes Clustering.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="KubernetesOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureKubernetesClustering([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            var kubernetesOptions = configurationSection.Get<KubernetesClientOptions>() ?? new KubernetesClientOptions();

            return builder.UseKubeGatewayListProvider(options =>
            {
                options.Group = kubernetesOptions.Group;
                options.CertificateData = kubernetesOptions.CertificateData;
                options.APIEndpoint = kubernetesOptions.ApiEndpoint;
                options.APIToken = kubernetesOptions.ApiToken;
            });
        }
    }
}