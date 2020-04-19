using GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Extensions
{
    /// <summary>
    /// Tracing Configuration Builder Extensions.
    /// </summary>
    public static class TracingConfigurationBuilderExtensions
    {
        private const string ExporterName = "Zipkin";

        /// <summary>
        /// Registers the Zipkin Exporter.
        /// </summary>
        /// <param name="builder">The <see cref="TracingConfigurationBuilder" />.</param>
        /// <param name="configurationSectionName">The Configuration section name. </param>
        /// <returns>The <see cref="TracingConfigurationBuilder" />.</returns>
        public static TracingConfigurationBuilder RegisterZipkin([NotNull] this TracingConfigurationBuilder builder, string configurationSectionName = ExporterName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = builder.TracingConfiguration.GetSection($"Exporters:{configurationSectionName}").Get<ZipkinExporterOptions>();

            return options == null ? builder : builder.RegisterExporter(ExporterName, new ZipkinTracingExporterProvider(options));
        }
    }
}