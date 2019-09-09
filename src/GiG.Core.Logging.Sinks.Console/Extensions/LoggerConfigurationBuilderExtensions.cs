using GiG.Core.Logging.Abstractions;

namespace GiG.Core.Logging.Sinks.Console.Extensions
{
    /// <summary>
    /// Logger Configuration builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        private const string SinkName = "Console";
        
        /// <summary>
        /// Writes log events to <see cref="System.Console"/>.
        /// </summary>
        /// <param name="builder">Logger sink configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfigurationBuilder WriteToConsole(this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink(SinkName, new ConsoleLoggerSinkProvider());
    }
}