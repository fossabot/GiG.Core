namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    public static class LoggerConfigurationBuilderExtensions
    {
        public static LoggerConfigurationBuilder WriteToFluentd(this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink("Console", new FluentdLoggerSink(builder.Configuration));
    }
}