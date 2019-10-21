using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support Correlation Id functionality in Orleans.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCorrelationAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}