using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Distributed Tracing using OpenTelemetry.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="tracingConfigurationBuilder">>A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="TracingOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTracing(
            [NotNull] this IServiceCollection services, 
            Action<TracingConfigurationBuilder> tracingConfigurationBuilder, 
            [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddTracing(tracingConfigurationBuilder, configuration.GetSection(TracingOptions.DefaultSectionName));
        }

        /// <summary>
        /// Adds Distributed Tracing using OpenTelemetry.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="tracingConfigurationBuilder">>A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="TracingOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddTracing(
            [NotNull] this IServiceCollection services, 
            Action<TracingConfigurationBuilder> tracingConfigurationBuilder, 
            [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.AddOpenTelemetry(builder =>
            {
                builder
                    // Configure tracing exporters
                    .ConfigureTracing(tracingConfigurationBuilder, configurationSection)
                    // Configure tracing to collect incoming HTTP requests
                    .AddRequestAdapter()
                    // Configure tracing to collect outgoing HTTP requests
                    .AddDependencyAdapter();
            });

            return services;
        }
    }
}