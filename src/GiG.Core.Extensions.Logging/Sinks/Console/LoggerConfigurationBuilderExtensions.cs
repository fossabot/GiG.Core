namespace GiG.Core.Extensions.Logging.Sinks.Console
{
    public static class LoggerConfigurationBuilderExtensions
    {
        public static LoggerConfigurationBuilder WriteToConsole(this LoggerConfigurationBuilder builder) =>
            builder.RegisterSink("Console", new ConsoleLoggerSink());
    }
}