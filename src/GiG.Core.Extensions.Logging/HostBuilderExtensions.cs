using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GiG.Core.Extensions.Logging
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds the default logging implementation.
        /// </summary>
        /// <param name="builder">Host builder</param>
        /// <param name="configurationSectionName">Configuration section name</param>
        /// <returns></returns>
        public static IHostBuilder UseLogging([NotNull] this IHostBuilder builder, string configurationSectionName = "Logging")
        {
            builder.UseSerilog();
            builder.ConfigureServices((context, collections) => ConfigureLoggerService(context.Configuration, collections, configurationSectionName));

            return builder;
        }

        private static void ConfigureLoggerService(IConfiguration configuration, IServiceCollection collection, string configurationSectionName)
        {
            var loggingSection = configuration.GetSection(configurationSectionName);
            if (loggingSection == null)
            {
                throw new ArgumentNullException(nameof(configuration), "Logging information is missing");
            }

            var loggingConfiguration = loggingSection.Get<LoggerConfiguration>();
            if (loggingConfiguration == null)
            {
                throw new ArgumentNullException(nameof(configurationSectionName), "Logging section is not valid");
            }
            
            var loggerConfiguration = new Serilog.LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration, configurationSectionName);

            if (loggingConfiguration.LogToConsole)
            {
                loggerConfiguration.WriteTo.Console();
            }

            Log.Logger = loggerConfiguration.CreateLogger();

            collection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, true));
        }
    }
}