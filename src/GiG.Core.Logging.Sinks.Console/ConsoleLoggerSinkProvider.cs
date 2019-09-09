using GiG.Core.Logging.Abstractions;
using Serilog;
using Serilog.Configuration;

namespace GiG.Core.Logging.Sinks.Console
{
    internal class ConsoleLoggerSinkProvider : ILoggerSinkProvider
    {
        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.Console();
        }
    }
}