using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;

namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    /// <summary>
    /// Logger Configuration builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        private const string SinkName = "Fluentd";

        /// <summary>
        /// Writes log events to FluentD
        /// </summary>
        /// <param name="builder">Logger sink configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfigurationBuilder WriteToFluentd([NotNull] this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink(SinkName,
                new FluentdLoggerSinkProvider(builder.SinkConfiguration.GetSection(SinkName)));
    }
}