using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Configuration;

namespace GiG.Core.Logging.Abstractions
{
    /// <summary>
    /// Logging Configuration builder.
    /// </summary>
    public class LoggingConfigurationBuilder
    {
        /// <summary>
        /// The Sink configuration section.
        /// </summary>
        public IConfigurationSection SinkConfiguration { get; }

        /// <summary>
        /// The Logger configuration.
        /// </summary>
        public LoggerConfiguration LoggerConfiguration { get; }

        /// <summary>
        /// The Service Collection.
        /// </summary>
        public IServiceCollection Services { get; }

        private readonly LoggingOptions _loggingOptions;

        /// <summary>
        /// Logging Configuration builder.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="loggerConfiguration">The <see cref="LoggerConfiguration"/>.</param>
        /// <param name="configurationSection">The Logging Configuration section.</param>
        public LoggingConfigurationBuilder([NotNull] IServiceCollection services, [NotNull] LoggerConfiguration loggerConfiguration, [NotNull] IConfigurationSection configurationSection)
        {
            _loggingOptions = configurationSection.Get<LoggingOptions>();
            if (_loggingOptions == null)
            {
                throw new ConfigurationErrorsException(
                    $"Configuration section '{configurationSection.Key}' is not valid");
            }

            const string sectionName = nameof(_loggingOptions.Sinks);
            SinkConfiguration = configurationSection.GetSection(sectionName);
            if (SinkConfiguration == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{sectionName}' does not exist");
            }

            Services = services ?? throw new ArgumentNullException(nameof(services));
            LoggerConfiguration = loggerConfiguration ?? throw new ArgumentNullException(nameof(loggerConfiguration));
        }

        /// <summary>
        /// Register Sink provider.
        /// </summary>
        /// <param name="name">The Sink provider name.</param>
        /// <param name="sinkProvider">Logging sink provider instance.</param>
        /// <exception cref="ConfigurationErrorsException">Throws a <see cref="ConfigurationErrorsException"/> when Sink providers are not configured.</exception>
        public LoggingConfigurationBuilder RegisterSink([NotNull] string name, [NotNull] ILoggingSinkProvider sinkProvider)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException($"Missing {nameof(name)}.");
            if (sinkProvider == null) throw new ArgumentNullException(nameof(sinkProvider));
            
            if (_loggingOptions.Sinks == null)
            {
                throw new ConfigurationErrorsException("No sinks were configured.  Please add at least one sink provider");
            }

            if (!_loggingOptions.Sinks.TryGetValue(name, out var sinkOptions))
            {
                return this;
            }

            if (sinkOptions.IsEnabled)
            {
                sinkProvider.RegisterSink(LoggerConfiguration.WriteTo);
            }

            return this;
        }
    }
}