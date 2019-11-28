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
    /// <inheritdoc />
    internal class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _consumer;

        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;

        private bool _disposed;

        internal KafkaConsumer([NotNull] IKafkaBuilderOptions<TKey, TValue> kafkaBuilderOptions, [NotNull] ILogger<KafkaConsumer<TKey, TValue>> logger)
        {
            var kafkaBuilderOptions1 = kafkaBuilderOptions ?? throw new ArgumentNullException(nameof(kafkaBuilderOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var config = new ConsumerConfig(kafkaBuilderOptions1.KafkaProviderOptions.AdditionalConfiguration)
            {
                BootstrapServers = kafkaBuilderOptions1.KafkaProviderOptions.BootstrapServers,
                GroupId = kafkaBuilderOptions1.KafkaProviderOptions.GroupId,
                AutoOffsetReset = kafkaBuilderOptions1.KafkaProviderOptions.AutoOffsetReset,
                EnableAutoCommit = kafkaBuilderOptions1.KafkaProviderOptions.EnableAutoCommit,
                SaslUsername = kafkaBuilderOptions1.KafkaProviderOptions.SaslUsername,
                SaslPassword = kafkaBuilderOptions1.KafkaProviderOptions.SaslPassword,
                SecurityProtocol = kafkaBuilderOptions1.KafkaProviderOptions.SecurityProtocol,
                SaslMechanism = kafkaBuilderOptions1.KafkaProviderOptions.SaslMechanism
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(kafkaBuilderOptions1.Serializers.ValueDeserializer)
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .SetPartitionsAssignedHandler((_, e) => _logger.LogDebug($"Assigned partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .SetPartitionsRevokedHandler((_, e) => _logger.LogDebug($"Revoked partitions: [{string.Join(", ", e.Select(x => x.Partition))}]"))
                .Build();

            _consumer.Subscribe(kafkaBuilderOptions1.KafkaProviderOptions.Topic);
        }

        /// <inheritdoc />
        public IKafkaMessage<TKey, TValue> Consume(CancellationToken cancellationToken = default)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);
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

        /// <inheritdoc />
        public void Commit(IKafkaMessage<TKey, TValue> message)
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