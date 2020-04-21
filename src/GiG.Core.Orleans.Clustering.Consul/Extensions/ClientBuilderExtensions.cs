using GiG.Core.Orleans.Clustering.Consul.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Clustering.Consul.Extensions
{
    /// <summary>
    /// The <see cref="IClientBuilder" /> Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Configures Consul Clustering.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="ConsulOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.ConfigureConsulClustering(configuration.GetSection(ConsulOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures Consul Clustering.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="ConsulOptions"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureConsulClustering([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            var consulOptions = configurationSection.Get<ConsulOptions>() ?? new ConsulOptions();

            return builder.UseConsulClustering(options =>
            {
                options.Address = new Uri(consulOptions.Address);
                options.KvRootFolder = consulOptions.KvRootFolder;
            });
        }
    }
}