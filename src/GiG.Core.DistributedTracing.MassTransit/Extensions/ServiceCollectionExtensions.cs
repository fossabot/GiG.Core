using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.MassTransit.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.DistributedTracing.MassTransit.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services to support Correlation ID functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCorrelationAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddMassTransitContext();
            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}
