using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Telemetry
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
        /// The Service Collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Is Exporter Configured
        /// </summary>
        public bool IsExporterConfigured { get; private set; }

        /// <summary>
        /// Tracing Configuration builder.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="exporters">List of exporters.</param>
        public TracingConfigurationBuilder([NotNull] IServiceCollection services, [NotNull] IDictionary<string, BasicExporterOptions> exporters)
        {
            Exporters = exporters ?? throw new ArgumentNullException(nameof(exporters));
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Register Tracing Exporter.
        /// </summary>
        /// <param name="name">The Tracing Exporter name.</param>
        /// <param name="tracingExporter">The <see cref="ITracingExporter"/>.</param>
        public TracingConfigurationBuilder RegisterExporter([NotNull] string name, [NotNull] ITracingExporter tracingExporter)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (tracingExporter == null) throw new ArgumentNullException(nameof(tracingExporter));

            if (IsExporterConfigured)
            {
                return this;
            }

            if (!Exporters.TryGetValue(name, out var exporterOptions))
            {
                return this;
            }

            tracingExporter.RegisterExporter(exporterOptions.Exporter);
            IsExporterConfigured = true;

            return this;
        }
    }
}