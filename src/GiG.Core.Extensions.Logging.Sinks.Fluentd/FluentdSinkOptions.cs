using GiG.Core.Logging.Abstractions;

namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    internal class FluentdSinkOptions : BasicSinkOptions
    {
        public string Hostname { get; set; } = "localhost";

        public int Port { get; set; } = 24224;
    }
}