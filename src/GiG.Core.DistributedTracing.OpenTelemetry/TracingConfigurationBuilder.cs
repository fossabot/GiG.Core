using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace.Configuration;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.OpenTelemetry
{
    /// <summary>
    /// Tracing Configuration builder.
    /// </summary>
    public class TracingConfigurationBuilder
    {
        /// <summary>
        /// The Exporters.
        /// </summary>
        public IDictionary<string, BasicExporterOptions> Exporters { get; }

        /// <summary>
        /// The Trace Builder.
        /// </summary>
        public TracerBuilder TracerBuilder { get; }

        /// <summary>
        /// At least one Tracing Exporter Configured.
        /// </summary>
        public bool IsConfigured { get; private set; }

        /// <summary>
        /// Tracing Configuration
        /// </summary>
        public IConfiguration TracingConfiguration { get; }

        /// <summary>
        /// Tracing Configuration builder.
        /// </summary>
        /// <param name="tracerBuilder">The <see cref="TracerBuilder"/>.</param>
        /// <param name="exporters"> A <see><cref>IDictionary{string, BasicExporterOptions}</cref></see> /> of exporters.</param>
        /// <param name="configurationSection">The <see cref="IConfiguration"/>.</param>
        public TracingConfigurationBuilder([NotNull] TracerBuilder tracerBuilder, [NotNull] IDictionary<string, BasicExporterOptions> exporters,
            [NotNull] IConfiguration configurationSection)
        {
            Exporters = exporters ?? throw new ArgumentNullException(nameof(exporters));
            TracingConfiguration = configurationSection ?? throw new ArgumentNullException(nameof(configurationSection));
            TracerBuilder = tracerBuilder ?? throw new ArgumentNullException(nameof(tracerBuilder));
        }

        /// <summary>
        /// Register Tracing Exporter.
        /// </summary>
        /// <param name="name">The Tracing Exporter name.</param>
        /// <param name="tracingExporter">The <see cref="ITracingExporter"/>.</param>
        public TracingConfigurationBuilder RegisterExporter([NotNull] string name, [NotNull] ITracingExporter tracingExporter)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (tracingExporter == null) throw new ArgumentNullException(nameof(tracingExporter));

            if (!Exporters.TryGetValue(name, out var exporterOptions))
            {
                return this;
            }

            if (exporterOptions.IsEnabled != true)
            {
                return this;
            }

            tracingExporter.RegisterExporter(TracerBuilder);
            IsConfigured = true;

            return this;
        }
    }
}