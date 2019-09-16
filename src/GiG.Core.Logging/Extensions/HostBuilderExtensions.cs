using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Configuration;

namespace GiG.Core.Logging.Extensions
{
    /// <summary>
    /// Host builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures logging sinks and enrichers.
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <param name="configureLogging">The delegate for configuring the <see cref="LoggingConfigurationBuilder" />.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            Action<LoggingConfigurationBuilder> configureLogging,
            [NotNull] string sectionName = LoggingOptions.DefaultSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (sectionName == null) throw new ArgumentNullException(nameof(sectionName));

            return builder
                .ConfigureServices((context, services) =>
                    ConfigureLoggingInternal(context, services, configureLogging, sectionName))
                .UseSerilog();
        }

        private static void ConfigureLoggingInternal(HostBuilderContext context, IServiceCollection services,
            Action<LoggingConfigurationBuilder> configureLogging, string sectionName)
        {
            var configuration = context.Configuration;

            var configurationSection = configuration.GetSection(sectionName);
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{sectionName}' does not exist");
            }

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration, sectionName);

            var loggerConfigurationBuilder =
                new LoggingConfigurationBuilder(services, loggerConfiguration, configurationSection);

            configureLogging?.Invoke(loggerConfigurationBuilder);

            Log.Logger = loggerConfiguration.CreateLogger();
            services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(Log.Logger, true));
        }
    }
}