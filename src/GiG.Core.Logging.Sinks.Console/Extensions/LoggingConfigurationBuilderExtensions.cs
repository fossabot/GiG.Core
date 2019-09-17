using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.Console.Internal;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Sinks.Console.Extensions
{
    /// <summary>
    /// Logging Configuration builder extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        private const string SinkName = "Console";

        /// <summary>
        /// Writes log events to <see cref="T:System.Console"/>.
        /// </summary>
        /// <param name="builder">Logging sink configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggingConfigurationBuilder WriteToConsole([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.RegisterSink(SinkName, new ConsoleLoggingSinkProvider());
        }
    }
}