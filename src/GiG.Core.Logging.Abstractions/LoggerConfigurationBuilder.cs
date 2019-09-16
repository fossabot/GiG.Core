using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Configuration;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logger Configuration builder.
    /// </summary>
    public class LoggerConfigurationBuilder
    {
        /// <summary>
        /// Sink Configuration section.
        /// </summary>
        public IConfigurationSection SinkConfiguration { get; }

        /// <summary>
        /// Logger Configuration.
        /// </summary>
        public LoggerConfiguration LoggerConfiguration { get; }

        /// <summary>
        /// Service Collection.
        /// </summary>
        public IServiceCollection Services { get; }

        private readonly LoggerOptions _loggerOptions;

        /// <summary>
        /// Logger Configuration builder.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="loggerConfiguration">Logger Configuration.</param>
        /// <param name="configurationSection">Logger Configuration section.</param>
        public LoggerConfigurationBuilder([NotNull] IServiceCollection services, [NotNull] LoggerConfiguration loggerConfiguration,
            [NotNull] IConfigurationSection configurationSection)
        {
            _loggerOptions = configurationSection.Get<LoggerOptions>();
            if (SinkConfiguration == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Key}' is not valid");
            }

            const string sectionName = nameof(_loggerOptions.Sinks);
            SinkConfiguration = configurationSection.GetSection(sectionName);
            if (SinkConfiguration == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{sectionName}' does not exist");
            }

            Services = services ?? throw new ArgumentNullException(nameof(services));
            LoggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        /// <summary>
        /// Register sink provider.
        /// </summary>
        /// <param name="name">Sink provider name.</param>
        /// <param name="sinkProvider">Logger sink provider instance.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Throws application exception when Sink providers are not configured.</exception>
        public LoggerConfigurationBuilder RegisterSink([NotNull]  string name, [NotNull] ILoggerSinkProvider sinkProvider)
        {
            if (_loggerOptions.Sinks == null)
            {
                throw new ApplicationException($"No sinks were configured.  Please add at least one sink provider");
            }

            if (!_loggerOptions.Sinks.TryGetValue(name, out var sinkOptions))
            {
                throw new ConfigurationErrorsException($"Logging sink '{name}' does not exist");
            }

            if (sinkOptions.IsEnabled)
            {
                sinkProvider?.RegisterSink(LoggerConfiguration.WriteTo);
            }

            return this;
        }
    }
}