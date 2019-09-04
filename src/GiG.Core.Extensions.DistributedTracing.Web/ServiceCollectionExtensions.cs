using CorrelationId;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CorrelationContextAccessor = GiG.Core.DistributedTracing.Web.CorrelationContextAccessor;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.CorrelationId.ICorrelationContextAccessor;

namespace GiG.Core.Extensions.DistributedTracing.Web
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
            CorrelationIdServiceExtensions.AddCorrelationId(services);

            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}