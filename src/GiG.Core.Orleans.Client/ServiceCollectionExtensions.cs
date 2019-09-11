using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System.Reflection;

namespace GiG.Core.Orleans.Client
{
    /// <summary>
    /// Service Collection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Creates and registers a new <see cref="IClusterClient"/> with default options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration which will be used to set the options for the client.</param>
        /// <param name="assemblies">The Assemblies which will be added to the cluster client.</param>
        public static void AddDefaultClusterClient(this IServiceCollection services,
            IConfiguration configuration,
            params Assembly[] assemblies)
        {
            new ClusterClientBuilder(services)
                .WithClusterOptions(configuration)
                .WithAssemblies(assemblies)
                .Register();
        }
    }
}