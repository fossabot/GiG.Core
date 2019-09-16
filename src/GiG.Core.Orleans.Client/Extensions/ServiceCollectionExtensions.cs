using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;

namespace GiG.Core.Orleans.Client.Extensions
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
        /// <param name="configureClient">The configuration which will be used to set the options for the client.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddClusterClient(this IServiceCollection services, Action<ClientBuilder, IServiceProvider> configureClient)
        {
            var builder = new ClientBuilder();
            
            configureClient?.Invoke(builder, services.BuildServiceProvider());
            
            return services.AddSingleton(builder.BuildAndConnect());
        }

        /// <summary>
        /// Creates and registers a new <see cref="IClusterClient"/> with default options.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureClient">The configuration which will be used to set the options for the client.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddClusterClient(this IServiceCollection services, Action<ClientBuilder> configureClient)
        {
            var builder = new ClientBuilder();

            configureClient?.Invoke(builder);

            return services.AddSingleton(builder.BuildAndConnect());
        }
    }
}