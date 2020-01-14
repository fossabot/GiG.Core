using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Configuration;
using System.Linq;

namespace GiG.Core.DistributedTracing.OpenTelemetry
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class TracerBuilderExtensions
    {
        /// <summary>
        /// Configures Tracing Exporters.
        /// </summary>
        /// <param name="tracerBuilder">The <see cref="TracerBuilder"/>.</param>
        /// <param name="tracingConfigurationBuilder">>A delegate that is used to configure the <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="sectionName">The configuration section name.</param>
        /// <returns>The <see cref="TracerBuilder"/>.</returns>
        public static TracerBuilder ConfigureTracing([NotNull] this TracerBuilder tracerBuilder, Action<TracingConfigurationBuilder> tracingConfigurationBuilder, IConfiguration configuration, string sectionName)
        {
            if (tracerBuilder == null) throw new ArgumentNullException(nameof(tracerBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            var configurationSection = configuration.GetSection(sectionName);

            var tracingOptions = configurationSection.Get<TracingOptions>();
            if (tracingOptions?.IsEnabled != true)
            {
                return tracerBuilder;
            }

            if (tracingOptions?.Exporters?.Any() != true)
            {
                throw new ConfigurationErrorsException("No tracing exporters were configured.  Please add at least one tracing exporter");
            }

            var builder = new TracingConfigurationBuilder(tracerBuilder, tracingOptions.Exporters, configurationSection);

            tracingConfigurationBuilder?.Invoke(builder);

            if (!builder.IsConfigured)
            {
                throw new ConfigurationErrorsException("Tracing is enabled but no tracing exporters were configured.");
            }

            return tracerBuilder;
        }
    }
}