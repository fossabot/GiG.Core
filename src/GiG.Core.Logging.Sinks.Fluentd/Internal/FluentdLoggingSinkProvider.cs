using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;

namespace GiG.Core.Logging.Sinks.Fluentd.Internal
{
    internal class FluentdLoggingSinkProvider : ILoggingSinkProvider
    {
        private readonly FluentdSinkOptions _options;
        
        public FluentdLoggingSinkProvider(IConfiguration configurationSection)
        {
            _options = configurationSection.Get<FluentdSinkOptions>() ?? new FluentdSinkOptions();
        }

        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.Fluentd(_options.Hostname, _options.Port);
        }
    }
}