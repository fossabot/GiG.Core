using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Service Collection Extensions for Hosting
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registration of Metadata Accessor for Application
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddApplicationMetadataAccessor([NotNull] this IServiceCollection services)
        {
            services.TryAddSingleton<IApplicationMetadataAccessor, ApplicationMetadataAccessor>();

            return services;
        }
    }
}