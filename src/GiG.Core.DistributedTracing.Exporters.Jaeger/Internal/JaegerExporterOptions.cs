using GiG.Core.DistributedTracing.Telemetry.Abstractions;

namespace GiG.Core.DistributedTracing.Exporters.Jaeger.Internal
{
    internal class JaegerExporterOptions : BasicExporterOptions
    {
        public string ServiceName { get; set; }
 
        public string AgentHost { get; set; } = "localhost";

        public int AgentPort { get; set; } = 6831;

        public int? MaxPacketSize { get; set; } = new int?(65000);
    }
}