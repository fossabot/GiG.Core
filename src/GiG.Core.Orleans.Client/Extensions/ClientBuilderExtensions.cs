using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Client.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Builds and connects the client.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <returns>The <see cref="IClusterClient"/>.</returns>
        public static IClusterClient BuildAndConnect([NotNull] this IClientBuilder clientBuilder)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            
            var clusterClient = clientBuilder.Build();
            
            clusterClient
                .Connect(CreateRetryFilter())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return clusterClient;
        }

        /// <summary>
        /// Sets the Cluster settings using an <see cref="IConfigurationSection"/>.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">An <see cref="IConfigurationSection"/> to configure the provided <see cref="IClientBuilder"/>.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureCluster([NotNull] this IClientBuilder clientBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");
            
            return clientBuilder.Configure<ClusterOptions>(configurationSection);
        }

        /// <summary>
        /// Sets the Cluster settings using an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">An <see cref="IConfiguration"/> to configure the provided <see cref="IClientBuilder"/>.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureCluster([NotNull] this IClientBuilder clientBuilder, [NotNull] IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var configurationSection = configuration.GetSection(Constants.ClusterDefaultSectionName);
            if (configurationSection?.Exists() != true)
            {
                throw new ConfigurationErrorsException($"Configuration section '{Constants.ClusterDefaultSectionName}' is incorrect.");
            }

            clientBuilder.Configure<ClusterOptions>(configurationSection);

            return clientBuilder;
        }

        /// <summary>
        /// Sets the Cluster settings using an <see cref="IConfiguration"/>.
        /// The configuration section is loaded from the section with the specified cluster name.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="clusterName">The Name of the Cluster being Configured.</param>
        /// <param name="configuration">An <see cref="IConfiguration"/> to configure the provided <see cref="IClientBuilder"/>.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureCluster([NotNull] this IClientBuilder clientBuilder, [NotNull] string clusterName, [NotNull] IConfiguration configuration)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(clusterName)) throw new ArgumentException($"'{nameof(clusterName)}' must not be null, empty or whitespace.", nameof(clusterName));

            var configurationSection = configuration.GetSection($"{Constants.ClusterDefaultSectionName}:{clusterName}");
            if (configurationSection?.Exists() != true)
            {
                throw new ConfigurationErrorsException($"Configuration section '{Constants.ClusterDefaultSectionName}' is incorrect.");
            }

            clientBuilder.Configure<ClusterOptions>(configurationSection);
            return clientBuilder;
        }

        /// <summary>
        /// Adds Assemblies to the Cluster Client Builder.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/> array which will be added to the cluster client.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureAssemblies([NotNull] this IClientBuilder clientBuilder, [NotNull] params Assembly[] assemblies)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            
            return clientBuilder.ConfigureApplicationParts(parts =>
            {
                foreach (var assembly in assemblies)
                {
                    parts.AddApplicationPart(assembly).WithReferences();
                }
            });
        }

        /// <summary>
        /// Adds Assemblies to Cluster Client Builder with references.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="types">The <see cref="Type"/> array which will be added to the cluster client.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder AddAssemblies([NotNull] this IClientBuilder clientBuilder, [NotNull] params Type[] types)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (types == null) throw new ArgumentNullException(nameof(types));

            return clientBuilder.ConfigureApplicationParts(parts =>
            {
                foreach (var type in types)
                {
                    parts.AddApplicationPart(type.Assembly).WithReferences();
                }
            });
        }

        private static Func<Exception, Task<bool>> CreateRetryFilter(int maxAttempts = 5)
        {
            var attempt = 0;
            return RetryFilter;

            async Task<bool> RetryFilter(Exception exception)
            {
                attempt++;
                if (attempt > maxAttempts)
                {
                    return false;
                }

                await Task.Delay(TimeSpan.FromSeconds(4));

                return true;
            }
        }
    }
}