using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Clustering.Kubernetes;
using Orleans.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="KubernetesOptions" /> will bind against.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder ConfigureKubernetesClustering([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return siloBuilder.ConfigureKubernetesClustering(configuration.GetSection(KubernetesOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="KubernetesOptions" /> will bind against.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder ConfigureKubernetesClustering([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            var kubernetesOptions = configurationSection.Get<KubernetesSiloOptions>() ?? new KubernetesSiloOptions();

            return
                  siloBuilder.UseKubeMembership(options =>
                  {
                      options.Group = kubernetesOptions.Group;
                      options.CertificateData = kubernetesOptions.CertificateData;
                      options.APIEndpoint = kubernetesOptions.ApiEndpoint;
                      options.APIToken = kubernetesOptions.ApiToken;
                      options.CanCreateResources = kubernetesOptions.CanCreateResources;
                      options.DropResourcesOnInit = kubernetesOptions.DropResourcesOnInit;
                  });
        }
    }
}