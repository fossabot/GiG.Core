using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Exceptions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Helpers;
using GiG.Core.Messaging.Kafka.Sample.Interfaces;
using GiG.Core.Messaging.Kafka.Sample.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Sample.Services
{
    public class CreatePersonService : ICreatePersonService
    {
        private readonly IKafkaProducer<string, PersonCreated> _kafkaProducer;
        private readonly ILogger<ConsumerService> _logger;

        
        public CreatePersonService(IKafkaProducer<string, PersonCreated> kafkaProducer, ILogger<ConsumerService> logger)
        {
            _kafkaProducer = kafkaProducer;
            _logger = logger;
        }

        public async Task HandleMessageAsync(IKafkaMessage<string, CreatePerson> consumedMessage)
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
                    DateOfBirth = consumedMessage.Value.DateOfBirth,
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
                await RetryHelper.RetryOnExceptionAsync<KafkaProducerException>(() => PublishEventAsync(personCreatedMessage));
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

        private async Task PublishEventAsync(KafkaMessage<string, PersonCreated> message)
        {
            try
            {
                await _kafkaProducer.ProduceAsync(message);
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);
               throw new KafkaProducerException();
            }
        }
    }
}