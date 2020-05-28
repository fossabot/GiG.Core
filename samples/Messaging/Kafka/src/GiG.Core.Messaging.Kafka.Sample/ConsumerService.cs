using Bogus;
using GiG.Core.Messaging.Kafka.Abstractions;
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
        private readonly IKafkaConsumer<string, CreatePerson> _kafkaConsumer;
        private readonly IKafkaProducer<string, PersonCreated> _kafkaProducer;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(IKafkaConsumer<string, CreatePerson> kafkaConsumer, IKafkaProducer<string, PersonCreated> kafkaProducer, ILogger<ConsumerService> logger)
        {
            _kafkaConsumer = kafkaConsumer;
            _kafkaProducer = kafkaProducer;
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
                            count = 0;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, e.Message);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _kafkaConsumer.Dispose();
            }
        }

        private void HandleMessage(IKafkaMessage<string, CreatePerson> consumedMessage)
        {
            try
            {
                // Validation
                ValidateMessage(consumedMessage);

                // Add Idempotentcy Check -It's important to verify for possible duplicates of the same message

                // Process 
                var serializedValue = JsonConvert.SerializeObject(consumedMessage.Value);
                _logger.LogInformation("Consumed message in service [key: '{key} '] [value: '{serializedValue}']", consumedMessage.Key, serializedValue);

                foreach (var (key, value) in consumedMessage.Headers)
                {
                    _logger.LogInformation("Header: {key}\tValue: {value}", key, value);
                }

                var personCreated = new PersonCreated()
                {
                    Id = consumedMessage.Value.Id,
                    Address = consumedMessage.Value.Address,
                    Age = consumedMessage.Value.Age,
                    Name = consumedMessage.Value.Name,
                    Surname = consumedMessage.Value.Surname
                };

                var messageId = Guid.NewGuid().ToString();

                var personCreatedMessage = new KafkaMessage<string, PersonCreated>
                {
                    Key = personCreated.Id.ToString(),
                    Value = personCreated,
                    MessageId = messageId,
                    MessageType = nameof(PersonCreated)
                };

                // PublishEvent
                PublishEvent(personCreatedMessage);
            }
            catch (ValidationException ex)
            {
                // Add Handle for Poison Message
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private void ValidateMessage(IKafkaMessage<string, CreatePerson> consumedMessage)
        {
            if (consumedMessage.MessageType != nameof(CreatePerson))
            {
                throw new ValidationException("Message Type Incorrect");
            }

            if (string.IsNullOrEmpty(consumedMessage.Value.Id.ToString()))
            {
                throw new ValidationException("Incorrect Person Id");
            }
        }

        private async void PublishEvent(KafkaMessage<string, PersonCreated> message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                
                // keep trying to publish event 
                PublishEvent(message);
            }
        }
    }
}