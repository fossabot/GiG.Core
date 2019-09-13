using CorrelationId;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using CorrelationContextAccessor = GiG.Core.DistributedTracing.Web.Internal.CorrelationContextAccessor;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor;

namespace GiG.Core.DistributedTracing.Web.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support Correlation Id functionality.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddCorrelationId([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            CorrelationIdServiceExtensions.AddCorrelationId(services);
            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}