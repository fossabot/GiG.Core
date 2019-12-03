using Confluent.Kafka;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Producers
{
    /// <inheritdoc />
    internal class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly IKafkaBuilderOptions<TKey, TValue> _kafkaBuilderOptions;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;

        internal KafkaProducer([NotNull] IKafkaBuilderOptions<TKey, TValue> kafkaBuilderOptions, [NotNull] ILogger<KafkaProducer<TKey, TValue>> logger)
        {
            _kafkaBuilderOptions = kafkaBuilderOptions ?? throw new ArgumentNullException(nameof(kafkaBuilderOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var config = new ProducerConfig(_kafkaBuilderOptions.KafkaProviderOptions.AdditionalConfiguration)
            {
                BootstrapServers = _kafkaBuilderOptions.KafkaProviderOptions.BootstrapServers,
                MessageTimeoutMs = _kafkaBuilderOptions.KafkaProviderOptions.MessageTimeoutMs,
                SaslUsername = _kafkaBuilderOptions.KafkaProviderOptions.SaslUsername,
                SaslPassword = _kafkaBuilderOptions.KafkaProviderOptions.SaslPassword,
                SecurityProtocol = _kafkaBuilderOptions.KafkaProviderOptions.SecurityProtocol,
                SaslMechanism = _kafkaBuilderOptions.KafkaProviderOptions.SaslMechanism
            };

            _producer = new ProducerBuilder<TKey, TValue>(config)
                .SetValueSerializer(_kafkaBuilderOptions.Serializers)
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .Build();
        }

        /// <inheritdoc />
        public async Task ProduceAsync(IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            var message = _kafkaBuilderOptions.MessageFactory.BuildMessage(kafkaMessage);
            await ProduceAsync(message);
        }

        private async Task ProduceAsync(Message<TKey, TValue> message)
        {
            try
            {
                var deliveryReport = await _producer.ProduceAsync(_kafkaBuilderOptions.KafkaProviderOptions.Topic, message);
                _logger.LogDebug("Delivered 'key: {key}' - '{value}' to '{topicPartitionOffset}' with offset '{offset}'", deliveryReport.Key, deliveryReport.Value, deliveryReport.TopicPartitionOffset, deliveryReport.Offset);
            }
            catch (ProduceException<TKey, TValue> e)
            {
                _logger.LogError(e, "Delivery failed: {reason}", e.Error.Reason);
                throw;
            }
        }

        public void Dispose()
        {
            _logger.LogDebug("Disposing Kafka Producer [{producerName}] ...", _producer.Name);
            
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}