using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Trace.Configuration;
using System;

namespace GiG.Core.DistributedTracing.Exporters.Jaeger.Internal
{
   internal class JaegerTracingExporterProvider : ITracingExporter
    {
        private readonly JaegerExporterOptions _options;

        public JaegerTracingExporterProvider(JaegerExporterOptions options)
        {
            _options = options;
        }

        public void RegisterExporter(TracerBuilder builder)
        {
            builder.UseJaeger(o =>
            {
                o.ServiceName = _options.ServiceName;
                o.AgentHost = _options.AgentHost;
                o.AgentPort = _options.AgentPort;
                o.MaxPacketSize = _options.MaxPacketSize;
            });

        }
    }
}