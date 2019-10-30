using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.Console.Internal;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Sinks.Console.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        private const string SinkName = "Console";

        /// <summary>
        /// Writes Log Events to <see cref="System.Console"/>.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder WriteToConsole([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.RegisterSink(SinkName, new ConsoleLoggingSinkProvider());
        }
    }
}