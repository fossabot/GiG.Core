using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry
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
        /// <param name="tracingConfigurationBuilder">>A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="sectionName">The configuration section name.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTracing([NotNull] this IServiceCollection services, Action<TracingConfigurationBuilder> tracingConfigurationBuilder, [NotNull] IConfiguration configuration,
            [NotNull] string sectionName = TracingOptions.DefaultSectionName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));

            services.AddOpenTelemetry(builder =>
            {
                // configure tracing exporters
                builder.ConfigureTracing(tracingConfigurationBuilder, configuration, sectionName);
                // configure tracing to collect incoming HTTP requests
                builder.AddRequestCollector();
                // configure tracing to collect outgoing HTTP requests
                builder.AddDependencyCollector();
            });

            return services;
        }
    }
}