using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    internal class FluentdLoggerSink : ILoggerSink
    {
        private readonly FluentdSinkOptions _options;
        
        public FluentdLoggerSink(IConfiguration configurationSection)
        {
            _options = configurationSection.Get<FluentdSinkOptions>();
        }

        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.Fluentd(_options.HostName, _options.Port);
        }
    }
}