using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Telemetry.Abstractions
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
        /// At least one Tracing Exporter Configured.
        /// </summary>
        public bool IsConfigured { get; private set; }

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
            if (string.IsNullOrEmpty(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (tracingExporter == null) throw new ArgumentNullException(nameof(tracingExporter));
          
            if (!Exporters.TryGetValue(name, out var exporterOptions))
            {
                return this;
            }
            
            if (exporterOptions?.IsEnabled != true)
            {
                return this;
            }

            tracingExporter.RegisterExporter(name);
            IsConfigured = true;

            return this;
        }
    }
}