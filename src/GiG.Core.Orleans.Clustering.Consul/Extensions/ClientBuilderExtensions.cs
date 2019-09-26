using GiG.Core.Orleans.Clustering.Consul.Configurations;
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
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Consul options.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.ConfigureConsulClustering(configuration.GetSection(ConsulOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Consul options.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var consulOptions = configurationSection.Get<ConsulOptions>() ?? new ConsulOptions();

            return
                  builder.UseConsulClustering((options) =>
                  {
                      options.Address = new Uri(consulOptions.Address);
                      options.KvRootFolder = consulOptions.KvRootFolder;
                  });
        }
    }
}