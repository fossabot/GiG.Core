using Serilog.Configuration;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logging Sink Provider.
    /// </summary>
    public interface ILoggingSinkProvider
    {
        /// <summary>
        /// Register Sink provider.
        /// </summary>
        /// <param name="sinkConfiguration">The <see cref="LoggerSinkConfiguration"/>.</param>
        void RegisterSink(LoggerSinkConfiguration sinkConfiguration);
    }
}