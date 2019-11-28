using System;
using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.RabbitMQ.Sinks.RabbitMQ;

namespace GiG.Core.Logging.Sinks.RabbitMQ.Internal
{
    internal class RabbitMQLoggingSinkProvider : ILoggingSinkProvider
    {
        private readonly RabbitMQSinkOptions _options;

        public RabbitMQLoggingSinkProvider(IConfiguration configurationSection)
        {
            _options = configurationSection.Get<RabbitMQSinkOptions>();
        }
        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            if(!Enum.TryParse(_options.DeliveryMode, true, out RabbitMQDeliveryMode deliveryMode)) 
                throw new ArgumentException("Invalid delivery mode", nameof(_options.DeliveryMode));

            var rabbitMqClientConfiguration = new RabbitMQClientConfiguration()
            {
                DeliveryMode = deliveryMode,
                Exchange = _options.Exchange,
                ExchangeType = _options.ExchangeType,
                Heartbeat = _options.Heartbeat,
                Password = _options.Password,
                Port = _options.Port,
                Username = _options.Username,
                VHost = _options.VHost
            };
            rabbitMqClientConfiguration.Hostnames.Add(_options.Hostname);

            sinkConfiguration.RabbitMQ(rabbitMqClientConfiguration,new RabbitMQSinkConfiguration()
            {
                BatchPostingLimit = _options.BatchPostingLimit,
                Period = TimeSpan.FromSeconds(_options.PeriodInSeconds),
                TextFormatter = new JsonFormatter()
            });
        }
    }
}
