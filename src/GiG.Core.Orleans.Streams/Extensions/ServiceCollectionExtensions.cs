using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Orleans.Streams.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {      
        /// <summary>
        /// Creates and registers a new <see cref="IStreamFactory"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that more methods can be chained.</returns>
        public static IServiceCollection AddStreamFactory([NotNull] this IServiceCollection services)
        {
            services.TryAddSingleton<IStreamFactory, StreamFactory>();
            return services;
        }
    }
}