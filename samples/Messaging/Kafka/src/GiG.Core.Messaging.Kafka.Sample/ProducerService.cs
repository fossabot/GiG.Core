using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample
{
    public class ProducerService : IHostedService
    {
        private readonly IKafkaProducer<string, CreatePerson> _kafkaProducer;
        private readonly ILogger<ProducerService> _logger;

        public ProducerService(IKafkaProducer<string, CreatePerson> kafkaProducer, ILogger<ProducerService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Run(RunProducer, cancellationToken);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _kafkaProducer.Dispose();
            return Task.CompletedTask;
        }

        private async Task RunProducer()
        {
            for (var i = 0; i < 20; i++)
            {
                var person = CreatePerson.Generate;
                var messageId = Guid.NewGuid().ToString();

                var message = new KafkaMessage<string, CreatePerson>
                {
                    Key = person.Id.ToString(),
                    Value = person,
                    MessageId = messageId,
                    MessageType = nameof(CreatePerson)
                };

                // Send Command
                await ProduceMessage(message);
            }
        }

        private async Task ProduceMessage(KafkaMessage<string, CreatePerson> message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _kafkaProducer.Dispose();
                
            }
        }
    }
}