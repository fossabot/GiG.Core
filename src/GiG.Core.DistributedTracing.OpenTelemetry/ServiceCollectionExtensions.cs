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
        /// Adds the required services to support Distributed Tracing using OpenTelemetry.
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
            if (string.IsNullOrEmpty(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            services.AddOpenTelemetry(builder =>
            {
                builder
                    // Configure tracing exporters
                    .ConfigureTracing(tracingConfigurationBuilder, configuration, sectionName)
                    // Configure tracing to collect incoming HTTP requests
                    .AddRequestCollector()
                    // Configure tracing to collect outgoing HTTP requests
                    .AddDependencyCollector();
            });

            return services;
        }
    }
}