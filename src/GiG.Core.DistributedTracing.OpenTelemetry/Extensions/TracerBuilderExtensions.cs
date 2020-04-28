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
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="TracingOptions"/>.</param>
        /// <returns>The <see cref="TracerBuilder"/>.</returns>
        public static TracerBuilder ConfigureTracing(
            [NotNull] this TracerBuilder tracerBuilder, 
            Action<TracingConfigurationBuilder> tracingConfigurationBuilder, 
            IConfigurationSection configurationSection)
        {
            if (tracerBuilder == null) throw new ArgumentNullException(nameof(tracerBuilder));
            if (tracingConfigurationBuilder == null) throw new ArgumentNullException(nameof(tracingConfigurationBuilder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

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