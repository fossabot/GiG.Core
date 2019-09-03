namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    internal class FluentdSinkOptions : BasicSinkOptions
    {
        public string HostName { get; set; } = "localhost";

        public int Port { get; set; } = 24224;
    }
}