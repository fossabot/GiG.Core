using Confluent.Kafka;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;

namespace GiG.Core.Messaging.Kafka
{
    public class MessageFactory : IMessageFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public MessageFactory(IDateTimeProvider dateTimeProvider, ICorrelationContextAccessor correlationContextAccessor = null)
        {
            _dateTimeProvider = Guard.IsNotNull(dateTimeProvider, nameof(dateTimeProvider));
            _correlationContextAccessor = correlationContextAccessor ?? new EmptyCorrelationContextAccessor();
        }

        public virtual Message<TKey, TValue> BuildMessage<TKey, TValue>(IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.MessageTypeHeaderName, kafkaMessage.MessageType);
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.MessageIdHeaderName, kafkaMessage.MessageId);
            kafkaMessage.Headers.AddOrUpdate(KafkaConstants.CorrelationIdHeaderName, _correlationContextAccessor?.CorrelationId.ToString());

            return new Message<TKey, TValue>
            {
                Key = kafkaMessage.Key,
                Value = kafkaMessage.Value,
                Timestamp = new Timestamp(_dateTimeProvider.GetNow()),
                Headers = kafkaMessage.Headers.ToKafkaHeaders()
            };
        }
    }
}