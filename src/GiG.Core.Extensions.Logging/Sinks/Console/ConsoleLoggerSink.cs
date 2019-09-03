using Serilog;
using Serilog.Configuration;

namespace GiG.Core.Extensions.Logging.Sinks.Console
{
    internal class ConsoleLoggerSink : ILoggerSink
    {
        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.Console();
        }
    }
}