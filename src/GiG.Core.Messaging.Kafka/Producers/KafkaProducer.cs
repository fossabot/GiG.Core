using Confluent.Kafka;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Producers
{
    /// <inheritdoc />
    internal class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly IProducer<TKey, TValue> _producer;
        private readonly IKafkaBuilderOptions<TKey, TValue> _kafkaBuilderOptions;
        private readonly ILogger<KafkaProducer<TKey, TValue>> _logger;
        private readonly Tracer _tracer;

        private bool _isDisposing;

        internal KafkaProducer([NotNull] IKafkaBuilderOptions<TKey, TValue> kafkaBuilderOptions, [NotNull] ILogger<KafkaProducer<TKey, TValue>> logger, Tracer tracer = null)
        {
            _kafkaBuilderOptions = kafkaBuilderOptions ?? throw new ArgumentNullException(nameof(kafkaBuilderOptions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tracer = tracer;

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
            var publishingActivity = new Activity(Constants.PublishActivityName);
            publishingActivity.Start();
           
            var message = _kafkaBuilderOptions.MessageFactory.BuildMessage(kafkaMessage, publishingActivity);
            
            await ProduceAsync(message, publishingActivity);
        }

        private async Task ProduceAsync(Message<TKey, TValue> message, Activity publishingActivity)
        {
            var span = _tracer?.StartSpanFromActivity($"{Constants.SpanPublishOperationNamePrefix}-{message.GetType().Name}", publishingActivity, SpanKind.Producer);
   
            try
            {
                var deliveryReport = await _producer.ProduceAsync(_kafkaBuilderOptions.KafkaProviderOptions.Topic, message);

                _logger.LogDebug("Delivered 'key: {key}' - '{value}' to '{topicPartitionOffset}' with offset '{offset}'", deliveryReport.Key, deliveryReport.Value, deliveryReport.TopicPartitionOffset,
                    deliveryReport.Offset);
            }
            catch (ProduceException<TKey, TValue> e)
            {
                _logger.LogError(e, "Delivery failed: {reason}", e.Error.Reason);
                throw;
            }
            finally
            {
                publishingActivity.Stop();
                span?.End();
            }
        }

        public void Dispose()
        {
            if (_isDisposing)
            {
                return;
            }

            _isDisposing = true;

            _logger.LogDebug("Disposing Kafka Producer [{producerName}] ...", _producer.Name);

            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer?.Dispose();
        }
    }
}