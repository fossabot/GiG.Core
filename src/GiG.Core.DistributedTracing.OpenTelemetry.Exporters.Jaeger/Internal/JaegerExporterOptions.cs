using GiG.Core.DistributedTracing.Telemetry.Abstractions;
using GiG.Core.Hosting;

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.Internal
{
    internal class JaegerExporterOptions : BasicExporterOptions
    {
        public string ServiceName { get; set; } =  ApplicationMetadata.Name;
 
        public string AgentHost { get; set; } = "localhost";

        public int AgentPort { get; set; } = 6831;

        public int? MaxPacketSize { get; set; } = new int?(65000);
    }
}