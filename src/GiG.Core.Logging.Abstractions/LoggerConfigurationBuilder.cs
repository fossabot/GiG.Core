using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

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
        public LoggerConfigurationBuilder(IServiceCollection services, LoggerConfiguration loggerConfiguration,
            IConfigurationSection configurationSection)
        {
            _loggerOptions = configurationSection.Get<LoggerOptions>();

            Services = services;
            LoggerConfiguration = loggerConfiguration;
            SinkConfiguration = configurationSection.GetSection(nameof(_loggerOptions.Sinks));
        }

        /// <summary>
        /// Register sink provider.
        /// </summary>
        /// <param name="name">Sink provider name.</param>
        /// <param name="sinkProvider">Logger sink provider instance.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException">Throws application exception when Sink providers are not configured.</exception>
        public LoggerConfigurationBuilder RegisterSink(string name, ILoggerSinkProvider sinkProvider)
        {
            if (_loggerOptions.Sinks == null)
            {
                throw new ApplicationException($"No sinks were configured.  Please add at least 1 sinkProvider provider");
            }

            if (!_loggerOptions.Sinks.TryGetValue(name, out var sinkOptions))
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