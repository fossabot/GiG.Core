using JetBrains.Annotations;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.Internal
{
   internal class JaegerTracingExporterProvider : ITracingExporter
    {
        private readonly JaegerExporterOptions _options;

        public JaegerTracingExporterProvider([NotNull] JaegerExporterOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public void RegisterExporter(TracerBuilder builder)
        {
            builder.UseJaeger(o =>
            {
                o.ServiceName = _options.ServiceName;
                o.AgentHost = _options.AgentHost;
                o.AgentPort = _options.AgentPort;
                o.MaxPacketSize = _options.MaxPacketSize;
                o.MaxFlushInterval = _options.MaxFlushInterval;
            });
        }
    }
}