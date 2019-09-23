using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.Fluentd.Internal;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Sinks.Fluentd.Extensions
{
    /// <summary>
    /// Logging Configuration builder extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        private const string SinkName = "Fluentd";

        /// <summary>
        /// Writes log events to FluentD
        /// </summary>
        /// <param name="builder">Logging sink configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggingConfigurationBuilder WriteToFluentd([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.RegisterSink(SinkName,
                new FluentdLoggingSinkProvider(builder.SinkConfiguration.GetSection(SinkName)));
        }
    }
}