﻿using GiG.Core.Messaging.Kafka.Abstractions;
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
        private readonly IKafkaProducer<string, Person> _kafkaProducer;
        private readonly ILogger<ProducerService> _logger;
        
        public ProducerService(IKafkaProducer<string, Person> kafkaProducer, ILogger<ProducerService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            Task.Run(() => RunProducer(cancellationToken), cancellationToken);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _kafkaProducer.Dispose();
            return Task.CompletedTask;
        }

        private async Task RunProducer(CancellationToken token = default)
        {
            try
            {
                for (var i = 0; i < 20; i++)
                {
                    var person = Person.Generate;
                    var messageId = Guid.NewGuid().ToString();

                    var message = new KafkaMessage<string, Person>
                    {
                        Key = "person",
                        Value = person,
                        MessageId = messageId,
                        MessageType = "Person.Created"
                    };

                    await _kafkaProducer.ProduceAsync(message);
                }
            }
            catch (Exception ex)
            {
                _kafkaProducer.Dispose();
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}