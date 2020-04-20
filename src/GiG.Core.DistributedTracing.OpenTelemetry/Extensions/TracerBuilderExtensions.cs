using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Configuration;
using System.Linq;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Extensions
{
    /// <summary>
    /// Tracer Builder Extensions.
    /// </summary>
    public static class TracerBuilderExtensions
    {
        /// <summary>
        /// Configures Tracing Exporters.
        /// </summary>
        /// <param name="tracerBuilder">The <see cref="TracerBuilder"/>.</param>
        /// <param name="tracingConfigurationBuilder">A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="configurationSectionName">The Configuration section name.</param>
        /// <returns>The <see cref="TracerBuilder"/>.</returns>
        public static TracerBuilder ConfigureTracing([NotNull] this TracerBuilder tracerBuilder, Action<TracingConfigurationBuilder> tracingConfigurationBuilder, IConfiguration configuration,
            string configurationSectionName = TracingOptions.DefaultSectionName)
        {
            if (tracerBuilder == null) throw new ArgumentNullException(nameof(tracerBuilder));
            if (tracingConfigurationBuilder == null) throw new ArgumentNullException(nameof(tracingConfigurationBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configurationSectionName)) throw new ArgumentException($"'{nameof(configurationSectionName)}' must not be null, empty or whitespace.", nameof(configurationSectionName));

            var configurationSection = configuration.GetSection(configurationSectionName);

            var tracingOptions = configurationSection.Get<TracingOptions>();
            if (tracingOptions?.IsEnabled != true)
            {
                return tracerBuilder;
            }

            if (tracingOptions.Exporters?.Any() != true)
            {
                throw new ConfigurationErrorsException("No tracing exporters were configured.  Please add at least one tracing exporter");
            }

            var builder = new TracingConfigurationBuilder(tracerBuilder, tracingOptions.Exporters, configurationSection);
            tracingConfigurationBuilder(builder);

            if (!builder.IsConfigured)
            {
                throw new ConfigurationErrorsException("Tracing is enabled but no tracing exporters were configured.");
            }

            return tracerBuilder;
        }
    }
}