using Serilog.Configuration;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logging Sink Provider.
    /// </summary>
    public interface ILoggingSinkProvider
    {
        /// <summary>
        /// Register sink provider.
        /// </summary>
        /// <param name="sinkConfiguration">Sink configuration.</param>
        void RegisterSink(LoggerSinkConfiguration sinkConfiguration);
    }
}