using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    internal class FluentdLoggerSinkProvider : ILoggerSinkProvider
    {
        private readonly FluentdSinkOptions _options;
        
        public FluentdLoggerSinkProvider(IConfiguration configurationSection)
        {
            _options = configurationSection.Get<FluentdSinkOptions>();
        }

        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.Fluentd(_options.Hostname, _options.Port);
        }
    }
}