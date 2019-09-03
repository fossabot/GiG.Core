using System;
using JetBrains.Annotations;
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
        /// <param name="builder">Host services</param>
        /// <param name="configureLogger">The delegate for configuring the <see cref="T:Serilog.LoggerConfiguration" /></param>
        /// <param name="sectionName">Configuration section name</param>
        /// <returns></returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            Action<LoggerConfigurationBuilder> configureLogger,
            [NotNull] string sectionName = "Logging")
        {
            builder
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    var configurationSection = configuration.GetSection(sectionName);
                    if (configurationSection == null)
                    {
                        throw new ArgumentException(
                            $"Configuration section '{sectionName}' does not exist",
                            nameof(sectionName));
                    }

                    var loggerConfiguration = new LoggerConfiguration()
                        .Enrich.FromLogContext()
                        .ReadFrom.Configuration(configuration, sectionName);

                    var loggerConfigurationBuilder = new LoggerConfigurationBuilder(services, loggerConfiguration, configurationSection);
                    configureLogger?.Invoke(loggerConfigurationBuilder);

                    Log.Logger = loggerConfiguration.CreateLogger();
                    services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(Log.Logger, true));

                })
                .UseSerilog();

            return builder;
        }
    }
}