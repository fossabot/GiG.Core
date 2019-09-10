using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.Console.Internal;
using JetBrains.Annotations;

namespace GiG.Core.Logging.Sinks.Console.Extensions
{
    /// <summary>
    /// Logger Configuration builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        private const string SinkName = "Console";
        
        /// <summary>
        /// Writes log events to <see cref="T:System.Console"/>.
        /// </summary>
        /// <param name="builder">Logger sink configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfigurationBuilder WriteToConsole([NotNull] this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink(SinkName, new ConsoleLoggerSinkProvider());
    }
}