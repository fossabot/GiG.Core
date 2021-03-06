﻿using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Configuration;

namespace GiG.Core.Logging.Extensions
{
    /// <summary>
    /// The <see cref="IHostBuilder" /> Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures logging sinks and enrichers.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <param name="configureLoggingConfigurationBuilder">A delegate that is used to configure the <see cref="LoggingConfigurationBuilder" />.</param>
        /// <param name="sectionName">The configuration section name.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureLogging([NotNull] this IHostBuilder builder, Action<LoggingConfigurationBuilder> configureLoggingConfigurationBuilder, [NotNull] string sectionName = LoggingOptions.DefaultSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(sectionName)) throw new ArgumentException($"'{nameof(sectionName)}' must not be null, empty or whitespace.", nameof(sectionName));

            return builder
                .ConfigureServices((context, services) => ConfigureLoggingInternal(context, services, configureLoggingConfigurationBuilder, sectionName))
                .UseSerilog(dispose: true);
        }

        private static void ConfigureLoggingInternal(HostBuilderContext context, IServiceCollection services,
            Action<LoggingConfigurationBuilder> configureLogging, string sectionName)
        {
            var configuration = context.Configuration;

            var configurationSection = configuration.GetSection(sectionName);
            if (configurationSection?.Exists() == false)
            {
                throw new ConfigurationErrorsException($"Configuration section '{sectionName}' is incorrect.");
            }
            
            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration, sectionName);

            var loggerConfigurationBuilder =
                new LoggingConfigurationBuilder(services, loggerConfiguration, configuration.GetSection(sectionName));

            configureLogging?.Invoke(loggerConfigurationBuilder);

            Log.Logger = loggerConfiguration.CreateLogger();
            services.AddLogging(loggerBuilder => loggerBuilder.AddSerilog(Log.Logger, true));
        }
    }
}