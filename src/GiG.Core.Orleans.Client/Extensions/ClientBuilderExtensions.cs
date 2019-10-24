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
        private const string ClusterDefaultSectionName = "Orleans:Cluster";

        /// <summary>
        /// Builds and connects the client.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <returns>The <see cref="IClusterClient"/>.</returns>
        public static IClusterClient BuildAndConnect([NotNull] this IClientBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var clusterClient = builder.Build();
            
            clusterClient
                .Connect(CreateRetryFilter())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            return clusterClient;
        }

        /// <summary>
        /// Configures the Cluster using an <see cref="IConfigurationSection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureCluster([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            
            return builder.Configure<ClusterOptions>(configurationSection);
        }

        /// <summary>
        /// Configures the Cluster using an <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureCluster([NotNull] this IClientBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            var configurationSection = configuration.GetSection(ClusterDefaultSectionName);
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{ClusterDefaultSectionName}' does not exist");
            }

            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }

        /// <summary>
        /// Adds Assemblies to the Cluster Client Builder.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/> array which will be added to the Cluster Client.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder ConfigureAssemblies([NotNull] this IClientBuilder builder, [NotNull] params Assembly[] assemblies)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            
            return builder.ConfigureApplicationParts(parts =>
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
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="types">The <see cref="Type"/> array which will be added to the Cluster Client.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder AddAssemblies([NotNull] this IClientBuilder builder, [NotNull] params Type[] types)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (types == null) throw new ArgumentNullException(nameof(types));

            return builder.ConfigureApplicationParts(parts =>
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