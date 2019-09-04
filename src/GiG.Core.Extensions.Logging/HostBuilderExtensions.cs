using System;
using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace GiG.Core.Extensions.Logging
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
        /// <param name="configureLogger">The delegate for configuring the <see cref="GiG.Core.Logging.Abstractions.LoggerConfigurationBuilder" />.</param>
        /// <param name="sectionName">Configuration section name.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder,
            Action<LoggerConfigurationBuilder> configureLogger,
            [NotNull] string sectionName = LoggerOptions.DefaultSectionName)
        {
            builder
                .ConfigureServices((context, services) =>
                    ConfigureLoggingInternal(context, services, configureLogger, sectionName))
                .UseSerilog();

            return builder;
        }

        private static void ConfigureLoggingInternal(HostBuilderContext context, IServiceCollection services,
            Action<LoggerConfigurationBuilder> configureLogger, string sectionName)
        {
            var configuration = context.Configuration;

            var configurationSection = configuration.GetSection(sectionName);
            if (configurationSection == null)
            {
                throw new ArgumentException($"Configuration section '{sectionName}' does not exist",
                    nameof(sectionName));
            }

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration, sectionName);

            var loggerConfigurationBuilder =
                new LoggerConfigurationBuilder(services, loggerConfiguration, configurationSection);
            configureLogger?.Invoke(loggerConfigurationBuilder);

            Log.Logger = loggerConfiguration.CreateLogger();
            services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(Log.Logger, true));
        }
    }
}