using GiG.Core.Logging.Abstractions;
using GiG.Core.Logging.Sinks.RabbitMQ.Internal;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Logging.Sinks.RabbitMQ.Extensions
{
    /// <summary>
    /// Logging Configuration Builder Extensions.
    /// </summary>
    public static class LoggingConfigurationBuilderExtensions
    {
        private const string SinkName = "RabbitMQ";

        /// <summary>
        /// Writes Log Events to RabbitMQ.
        /// </summary>
        /// <param name="builder">The <see cref="LoggingConfigurationBuilder" />.</param>
        /// <returns>The <see cref="LoggingConfigurationBuilder" />.</returns>
        public static LoggingConfigurationBuilder WriteToRabbitMQ([NotNull] this LoggingConfigurationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.RegisterSink(SinkName, new RabbitMQLoggingSinkProvider(builder.SinkConfiguration.GetSection(SinkName)));
        }
    }
}