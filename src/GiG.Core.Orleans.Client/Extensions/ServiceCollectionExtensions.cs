using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using System;

namespace GiG.Core.Orleans.Client.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Creates and registers a new <see cref="IClusterClient"/> with default options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
        /// <param name="configureClient">The <see cref="Action{ClientBuilder, IServiceProvider}"/> on which will be used to set the options for the client.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddClusterClient([NotNull] this IServiceCollection services, [NotNull] Action<ClientBuilder, IServiceProvider> configureClient)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var builder = new ClientBuilder();
            
            configureClient?.Invoke(builder, services.BuildServiceProvider());
            
            return services.AddSingleton(builder.BuildAndConnect());
        }

        /// <summary>
        /// Creates and registers a new <see cref="IClusterClient"/> with default options.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
        /// <param name="configureClient">The <see cref="Action{ClientBuilder}"/> on which will be used to set the options for the client.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddClusterClient([NotNull] this IServiceCollection services, [NotNull] Action<ClientBuilder> configureClient)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var builder = new ClientBuilder();

            configureClient?.Invoke(builder);

            return services.AddSingleton(builder.BuildAndConnect());
        }
    }
}