using Confluent.Kafka;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Providers.DateTime.Abstractions;
using JetBrains.Annotations;
using System;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class MessageFactory : IMessageFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <inheritdoc />
        public MessageFactory([NotNull] IDateTimeProvider dateTimeProvider, [NotNull] ICorrelationContextAccessor correlationContextAccessor)
        {
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
            _correlationContextAccessor = correlationContextAccessor ?? throw new ArgumentNullException(nameof(correlationContextAccessor));
        }
        
        /// <inheritdoc />
        public virtual Message<TKey, TValue> BuildMessage<TKey, TValue>([NotNull] IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            if (kafkaMessage == null) throw new ArgumentNullException(nameof(kafkaMessage));
            
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.MessageTypeHeaderName, kafkaMessage.MessageType);
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.MessageIdHeaderName, kafkaMessage.MessageId);
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.CorrelationIdHeaderName, _correlationContextAccessor?.Value.ToString());

            return new Message<TKey, TValue>
            {
                Key = kafkaMessage.Key,
                Value = kafkaMessage.Value,
                Timestamp = new Timestamp(_dateTimeProvider.Now),
                Headers = kafkaMessage.Headers.ToKafkaHeaders()
            };
        }
    }
}