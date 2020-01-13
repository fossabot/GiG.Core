using GiG.Core.DistributedTracing.Exporters.Jaeger.Internal;
using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using Microsoft.Extensions.Configuration;
using JetBrains.Annotations;
using System;

namespace GiG.Core.DistributedTracing.Exporters.Jaeger.External
{
    /// <summary>
    /// Tracing Configuration Builder Extensions.
    /// </summary>
    public static class TracingConfigurationBuilderExtensions
    {
        private const string ExporterName = "Jaeger";

        /// <summary>
        /// Register Jaeger Exporter.
        /// </summary>
        /// <param name="builder">The <see cref="TracingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="TracingConfigurationBuilder" />.</returns>
        public static TracingConfigurationBuilder RegisterJaeger([NotNull] this TracingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = builder.TracingConfiguration.GetSection($"Exporters:{ExporterName}").Get<JaegerExporterOptions>();

            if (options == null)
            {
                return builder;
            }
                
            return builder.RegisterExporter(ExporterName, new JaegerTracingExporterProvider(options));
        }
    }
}