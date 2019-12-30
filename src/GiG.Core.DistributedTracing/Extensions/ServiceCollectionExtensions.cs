using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services to support Distributed Tracing.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTracing([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddOpenTelemetry(builder =>
            {
                builder
                    // configure tracing to collect incoming HTTP requests
                    .AddRequestCollector()
                    // configure tracing to collect outgoing HTTP requests
                    .AddDependencyCollector();
            });

            return services;
        }
    }
}