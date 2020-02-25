using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Elasticsearch;

namespace GiG.Core.Logging.Sinks.Console.Internal
{
    internal class ConsoleLoggingSinkProvider : ILoggingSinkProvider
    {
        private readonly ConsoleSinkOptions _sinkOptions;
        
        public ConsoleLoggingSinkProvider(IConfiguration configurationSection)
        {
            _sinkOptions = configurationSection.Get<ConsoleSinkOptions>() ?? new ConsoleSinkOptions();
        }
        
        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            if (_sinkOptions.UseStructuredLogging) sinkConfiguration.Console(new ElasticsearchJsonFormatter());
            else sinkConfiguration.Console();
        }
    }
}