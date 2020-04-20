using JetBrains.Annotations;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Internal
{
    internal class ZipkinTracingExporterProvider : ITracingExporter
    {
        private readonly ZipkinExporterOptions _options;

        public ZipkinTracingExporterProvider([NotNull] ZipkinExporterOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void RegisterExporter(TracerBuilder builder)
        {
            builder.UseZipkin(o =>
            {
                o.ServiceName = _options.ServiceName;
                o.Endpoint = _options.Endpoint;
                o.TimeoutSeconds = _options.TimeoutSeconds;
                o.UseShortTraceIds = _options.UseShortTraceIds;
            });
        }
    }
}