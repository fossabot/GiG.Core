using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.File.Internal;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Sinks.File.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        private const string SinkName = "File";

        /// <summary>
        /// Writes Log Events to File.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder WriteToFile([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.RegisterSink(SinkName, new FileLoggingSinkProvider(builder.SinkConfiguration.GetSection(SinkName)));
        }
    }
}