using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Service Collection Extensions for Hosting.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registration of Metadata Accessor for Application
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returns the <see cref="IServiceCollection"/> so that more methods can be chained.</returns>
        public static IServiceCollection AddApplicationMetadataAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IApplicationMetadataAccessor, ApplicationMetadataAccessor>();

            return services;
        }
    }
}