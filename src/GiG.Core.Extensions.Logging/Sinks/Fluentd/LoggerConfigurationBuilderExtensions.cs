namespace GiG.Core.Extensions.Logging.Sinks.Fluentd
{
    public static class LoggerConfigurationBuilderExtensions
    {
        public static LoggerConfigurationBuilder WriteToFluentd(this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink("Fluentd", new FluentdLoggerSink(builder.Configuration.GetSection("Sinks").GetSection("Fluentd")));
    }
}