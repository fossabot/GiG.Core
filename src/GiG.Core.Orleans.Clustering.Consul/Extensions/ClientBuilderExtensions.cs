using GiG.Core.Orleans.Clustering.Consul.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Consul.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="ConsulOptions" /> will bind against.
        /// </summary>
        /// <param name="clientBuilder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder clientBuilder, [NotNull] IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return clientBuilder.ConfigureConsulClustering(configuration.GetSection(ConsulOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="ConsulOptions" /> will bind against.
        /// </summary>
        /// <param name="clientBuilder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder clientBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configurationSection?.Exists() != true) throw new ArgumentNullException(nameof(configurationSection));

            var consulOptions = configurationSection.Get<ConsulOptions>() ?? new ConsulOptions();

            return clientBuilder.UseConsulClustering(options =>
                  {
                      options.Address = new Uri(consulOptions.Address);
                      options.KvRootFolder = consulOptions.KvRootFolder;
                  });
        }
    }
}