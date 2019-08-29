using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

namespace GiG.Core.Extensions.Logging
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds the deafult logging implementation.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configurationSectionName"></param>
        /// <returns></returns>
        public static IHostBuilder UseLogging(this IHostBuilder builder, string configurationSectionName = "Logging")
        {
            builder.ConfigureServices((context, collections) => ConfigureLoggerService(context.Configuration, collections, configurationSectionName));
            return builder;
        }

        private static void ConfigureLoggerService(IConfiguration configuration, IServiceCollection collection, string configurationSectionName)
        {
            var loggingConfig = configuration.GetSection(configurationSectionName).Get<LoggerConfig>();
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(LevelConvert.ToSerilogLevel(loggingConfig.MinimumLogLevel));

            if (loggingConfig.LogToConsole)
            {
                loggerConfiguration.WriteTo.Console();
            }

            var logger = loggerConfiguration.CreateLogger();

            collection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logger, dispose: true));
        }
    }
}