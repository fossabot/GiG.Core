using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample
{
    public class ConsumerService : BackgroundService
    {
        private readonly IKafkaConsumer<string, Person> _kafkaConsumer;
        private readonly ILogger<ConsumerService> _logger;
        
        public ConsumerService(IKafkaConsumer<string, Person> kafkaConsumer, ILogger<ConsumerService> logger)
        {
            _kafkaConsumer = kafkaConsumer;
            _logger = logger;
        }

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            RunConsumer(cancellationToken);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _kafkaConsumer.Dispose();
            return Task.CompletedTask;
        }

        private void RunConsumer(CancellationToken token = default)
        {
            var count = 0;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var message = _kafkaConsumer.Consume(token);
                        HandleMessage(message);

                        if (count++ % 10 == 0)
                        {
                            _kafkaConsumer.Commit(message);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message );
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _kafkaConsumer.Dispose();
            }
        }

        private void HandleMessage(IKafkaMessage<string, Person> message)
        {
            var serializedValue = JsonConvert.SerializeObject(message.Value);
            _logger.LogInformation($"Consumed message in service [key: '{ message.Key }'] [value: '{ serializedValue }']");

            foreach (var (key, value) in message.Headers)
            {
                _logger.LogInformation($"Header: { key }\tValue: { value }");
            }
        }
    }
}