using Serilog.Configuration;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logger Sink Provider.
    /// </summary>
    public interface ILoggerSinkProvider
    {
        /// <summary>
        /// Register sink provider.
        /// </summary>
        /// <param name="sinkConfiguration">Sink configuration.</param>
        void RegisterSink(LoggerSinkConfiguration sinkConfiguration);
    }
}