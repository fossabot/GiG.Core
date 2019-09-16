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
        /// Builds and Connect the client.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <returns>The newly created client.</returns>
        public static IClusterClient BuildAndConnect(this IClientBuilder builder)
        {
            var clusterClient = builder.Build();

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
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureCluster(this IClientBuilder builder,
            IConfigurationSection configurationSection)
        {
            return builder.Configure<ClusterOptions>(configurationSection);
        }

        /// <summary>
        /// Sets the Cluster settings using an <see cref="IConfiguration"/>.
        /// </summary>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureCluster(this IClientBuilder builder, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(ClusterDefaultSectionName);
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException(
                    $"Configuration section '{ClusterDefaultSectionName}' does not exist");
            }

            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }

        /// <summary>
        /// Adds Assemblies to Cluster Client Builder with references.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="assemblies">The Assemblies which will be added to the cluster client.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder ConfigureAssemblies(this IClientBuilder builder, params Assembly[] assemblies)
        {
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
        /// <param name="types">The Assemblies which will be added to the cluster client.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder AddAssemblies(this IClientBuilder builder, params Type[] types)
        {
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