using GiG.Core.MassTransit.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.MassTransit.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services for the MassTransit Context.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddMassTransitContext([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddTransient<IMassTransitContextFactory, MassTransitContextFactory>();
            services.TryAddSingleton<IMassTransitContextAccessor, MassTransitContextAccessor>();

            return services;
        }
    }
}