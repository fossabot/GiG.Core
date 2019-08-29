using GiG.Common.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Extensions.Logging;

namespace GiG.Core.Logging.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseLogging(this IHostBuilder builder, string configurationSection = "Logging")
        {
            builder.ConfigureServices((context, collections) =>
            {
                var loggingConfig = context.Configuration.GetSection(configurationSection).Get<LoggerConfig>();

                ConfigureLoggerService(loggingConfig, collections);
            });

            return builder;
        }

        private static void ConfigureLoggerService(LoggerConfig loggerconfig, IServiceCollection collection)
        {           
            var logProvider = new LoggerConfiguration()
                .Enrich.FromLogContext()   
                .MinimumLevel.Is(LevelConvert.ToSerilogLevel(loggerconfig.MinimumLogLevel))
                .WriteTo.Console()
                .CreateLogger();

            collection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(logProvider, dispose: true));
        }
    }
}