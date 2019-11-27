using Confluent.Kafka;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GiG.Core.Messaging.Kafka.Consumers
{
    public class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _consumer;

        private readonly IKafkaBuilderOptions<TKey, TValue> _kafkaBuilderOptions;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;

        private bool _disposed = false;

        internal KafkaConsumer([NotNull] IKafkaBuilderOptions<TKey, TValue> kafkaBuilderOptions, [NotNull] ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            _kafkaBuilderOptions = kafkaBuilderOptions ?? throw new ArgumentNullException(nameof(kafkaBuilderOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var config = new ConsumerConfig(_kafkaBuilderOptions.KafkaProviderOptions.AdditionalConfiguration)
            {
                BootstrapServers = _kafkaBuilderOptions.KafkaProviderOptions.BootstrapServers,
                GroupId = _kafkaBuilderOptions.KafkaProviderOptions.GroupId,
                AutoOffsetReset = _kafkaBuilderOptions.KafkaProviderOptions.AutoOffsetReset,
                EnableAutoCommit = _kafkaBuilderOptions.KafkaProviderOptions.EnableAutoCommit,
                SaslUsername = _kafkaBuilderOptions.KafkaProviderOptions.SaslUsername,
                SaslPassword = _kafkaBuilderOptions.KafkaProviderOptions.SaslPassword,
                SecurityProtocol = _kafkaBuilderOptions.KafkaProviderOptions.SecurityProtocol,
                SaslMechanism = _kafkaBuilderOptions.KafkaProviderOptions.SaslMechanism
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(_kafkaBuilderOptions.Serializers.ValueDeserializer)
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .SetPartitionsAssignedHandler((_, e) => _logger.LogDebug($"Assigned partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .SetPartitionsRevokedHandler((_, e) => _logger.LogDebug($"Revoked partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .Build();

            _consumer.Subscribe(_kafkaBuilderOptions.KafkaProviderOptions.Topic);
        }

        public IKafkaMessage<TKey, TValue> Consume(CancellationToken token = default)
        {
            try
            {
                var consumeResult = _consumer.Consume(token);
                var kafkaMessage = (KafkaMessage<TKey, TValue>)consumeResult;

                _logger.LogDebug($"Consumed message 'key { consumeResult.Key } ' at: '{ consumeResult.TopicPartitionOffset }'.");

                return kafkaMessage;
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, $"ConsumeException occurred: { e.Error.Reason }");
                throw;
            }
            catch (KafkaException e)
            {
                _logger.LogError(e, $"KafkaException occurred: { e.Error.Reason }");
                throw;
            }
        }

        public void Commit(IKafkaMessage<TKey, TValue> message, CancellationToken token = default(CancellationToken))
        {
            var kafkaMessage = (KafkaMessage<TKey, TValue>)message;
            _consumer.Commit(new List<TopicPartitionOffset> { kafkaMessage.Offset });
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _logger.LogInformation("Disposing...");

                _consumer.Close();
                _consumer.Dispose();

                _disposed = true;
            }
        }
    }
}