using System;
using GiG.Core.Extensions.Logging.Sinks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace GiG.Core.Extensions.Logging
{
    public class LoggerConfigurationBuilder
    {
        internal IConfigurationSection Configuration { get; }
        public LoggerConfiguration LoggerConfiguration { get; }
        public IServiceCollection Services { get; }

        private readonly LoggerOptions _loggerOptions;

        public LoggerConfigurationBuilder(IServiceCollection services, LoggerConfiguration loggerConfiguration,
            IConfigurationSection loggerConfigurationSection)
        {
            Services = services;
            LoggerConfiguration = loggerConfiguration;
            Configuration = loggerConfigurationSection;
            _loggerOptions = loggerConfigurationSection.Get<LoggerOptions>();
        }

        internal LoggerConfigurationBuilder RegisterSink(string name, ILoggerSink sink)
        {
            if (_loggerOptions.Sinks == null)
            {
                throw new Exception($"Sinks were not configured");
            }

            if (!_loggerOptions.Sinks.TryGetValue(name, out var sinkOptions))
            {
                throw new Exception($"Sink '{name}' was not found");
            }

            if (sinkOptions.IsEnabled)
            {
                sink.RegisterSink(LoggerConfiguration.WriteTo);
            }

            return this;
        }
    }
}