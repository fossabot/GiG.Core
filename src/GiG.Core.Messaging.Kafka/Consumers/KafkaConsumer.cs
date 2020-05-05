﻿using Confluent.Kafka;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Consumers
{
    /// <inheritdoc />
    internal class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        private readonly ILogger<KafkaConsumer<TKey, TValue>> _logger;
        private readonly Tracer _tracer;
        private TelemetrySpan _span;

        private bool _disposed;

        internal KafkaConsumer([NotNull] IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] ILogger<KafkaConsumer<TKey, TValue>> logger, Tracer tracer = null)
        {
            var kafkaBuilderOptions = builderOptions ?? throw new ArgumentNullException(nameof(builderOptions));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tracer = tracer;

            var config = new ConsumerConfig(kafkaBuilderOptions.KafkaProviderOptions.AdditionalConfiguration)
            {
                BootstrapServers = kafkaBuilderOptions.KafkaProviderOptions.BootstrapServers,
                GroupId = kafkaBuilderOptions.KafkaProviderOptions.GroupId,
                AutoOffsetReset = kafkaBuilderOptions.KafkaProviderOptions.AutoOffsetReset,
                EnableAutoCommit = kafkaBuilderOptions.KafkaProviderOptions.EnableAutoCommit,
                SaslUsername = kafkaBuilderOptions.KafkaProviderOptions.SaslUsername,
                SaslPassword = kafkaBuilderOptions.KafkaProviderOptions.SaslPassword,
                SecurityProtocol = kafkaBuilderOptions.KafkaProviderOptions.SecurityProtocol,
                SaslMechanism = kafkaBuilderOptions.KafkaProviderOptions.SaslMechanism
            };

            _consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetValueDeserializer(kafkaBuilderOptions.Serializers.ValueDeserializer)
                .SetErrorHandler((_, e) => _logger.LogError(e.Reason))
                .SetLogHandler((_, e) => _logger.LogInformation(e.Message))
                .SetPartitionsAssignedHandler((_, e) => _logger.LogDebug("Assigned partitions: [{partitions}]", string.Join(", ", e.Select(x => x.Partition))))
                .SetPartitionsRevokedHandler((_, e) => _logger.LogDebug("Revoked partitions: [{partitions}]", string.Join(", ", e.Select(x => x.Partition))))
                .Build();

            _consumer.Subscribe(kafkaBuilderOptions.KafkaProviderOptions.Topic);
        }

        /// <inheritdoc />
        public IKafkaMessage<TKey, TValue> Consume(CancellationToken cancellationToken = default)
        {
            try
            {
                Activity.Current.Start();
                
                var consumeResult = _consumer.Consume(cancellationToken);
                var kafkaMessage = (KafkaMessage<TKey, TValue>)consumeResult;

                if (kafkaMessage.Headers?.Any() ?? false)
                {
                    if (kafkaMessage.Headers.ContainsKey(Constants.CorrelationIdHeaderName))
                    {
                        var parentActivityId = kafkaMessage.Headers[Constants.CorrelationIdHeaderName];

                        if (!string.IsNullOrEmpty(parentActivityId))
                        {
                            Activity.Current.SetParentId(parentActivityId);
                        }
                    }

                    foreach (var baggageItem in kafkaMessage.Headers)
                    {
                        if (baggageItem.Key != Constants.CorrelationIdHeaderName)
                        {
                            Activity.Current.AddBaggage(baggageItem.Key, baggageItem.Value);
                        }
                    }
                }

                _span = _tracer?.StartSpanFromActivity(Constants.SpanConsumeOperationNamePrefix, Activity.Current, SpanKind.Consumer);

                _logger.LogDebug("Consumed message 'key {key} ' at: '{partitionOffset}'.", consumeResult.Key, consumeResult.TopicPartitionOffset);

                return kafkaMessage;
            }
            catch (ConsumeException e)
            {
                _logger.LogError(e, "ConsumeException occurred: {reason}", e.Error.Reason);
                throw;
            }
            catch (KafkaException e)
            {
                _logger.LogError(e, "KafkaException occurred: {reason}", e.Error.Reason);
                throw;
            }
            finally
            {
                Activity.Current.Stop();
                _span?.End();
            }
        }

        /// <inheritdoc />
        public void Commit(IKafkaMessage<TKey, TValue> message)
        {
            var kafkaMessage = (KafkaMessage<TKey, TValue>) message;

            _span = _tracer?.StartSpanFromActivity(Constants.SpanConsumeCommitOperationNamePrefix, Activity.Current, SpanKind.Consumer);
            
            _consumer.Commit(new List<TopicPartitionOffset> {kafkaMessage.Offset});
            
            _span?.End();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _logger.LogDebug($"Disposing Kafka Consumer [{_consumer.MemberId}-{_consumer.Name}] ...");

            _consumer.Close();
            _consumer.Dispose();

            _disposed = true;
        }
    }
}