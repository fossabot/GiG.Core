using GiG.Core.Orleans.Clustering.Kubernetes.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Clustering.Kubernetes;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configures Kubernetes in Orleans.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Kubernetes options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureKubernetesClustering([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.ConfigureKubernetesClustering(configuration.GetSection(KubernetesClientOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures Kubernetes in Orleans.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Kubernetes options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureKubernetesClustering([NotNull] this ISiloBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var kubernetesOptions = configurationSection.Get<KubernetesSiloOptions>() ?? new KubernetesSiloOptions();

            return
                  builder.UseKubeMembership((options) =>
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
