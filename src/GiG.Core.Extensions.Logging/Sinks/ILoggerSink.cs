using Serilog.Configuration;

namespace GiG.Core.Extensions.Logging.Sinks
{
    internal interface ILoggerSink
    {
        void RegisterSink(LoggerSinkConfiguration sinkConfiguration);
    }
}