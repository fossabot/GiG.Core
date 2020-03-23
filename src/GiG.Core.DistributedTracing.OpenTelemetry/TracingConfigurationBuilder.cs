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
        private readonly IDictionary<string, BasicExporterOptions> _exporters;
        private readonly TracerBuilder _tracerBuilder;

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
        /// <param name="exporters"> A <see cref="IDictionary{String, BasicExporterOptions}" /> of exporters.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        public TracingConfigurationBuilder([NotNull] TracerBuilder tracerBuilder, [NotNull] IDictionary<string, BasicExporterOptions> exporters,
            [NotNull] IConfiguration configuration)
        {
            _exporters = exporters ?? throw new ArgumentNullException(nameof(exporters));
            _tracerBuilder = tracerBuilder ?? throw new ArgumentNullException(nameof(tracerBuilder));
            TracingConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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

            if (!_exporters.TryGetValue(name, out var exporterOptions))
            {
                return this;
            }

            if (exporterOptions.IsEnabled != true)
            {
                return this;
            }

            tracingExporter.RegisterExporter(_tracerBuilder);
            IsConfigured = true;

            return this;
        }
    }
}