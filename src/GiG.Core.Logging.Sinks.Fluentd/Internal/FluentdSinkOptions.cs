using GiG.Core.Logging.Abstractions;

namespace GiG.Core.Logging.Sinks.Fluentd.Internal
{
    internal class FluentdSinkOptions : BasicSinkOptions
    {
        public string Hostname { get; set; } = "localhost";

        public int Port { get; set; } = 24224;
    }
}