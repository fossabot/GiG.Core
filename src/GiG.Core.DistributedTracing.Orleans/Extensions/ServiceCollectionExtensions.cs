using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support Correlation Id functionality in Orleans.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns><see cref="IServiceCollection"/> so that more methods can be chained.</returns>
        public static IServiceCollection AddCorrelationId([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}