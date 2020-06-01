using Confluent.Kafka;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Providers.DateTime.Abstractions;
using JetBrains.Annotations;
using System;
using System.Diagnostics;
using AbstractionConstants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class MessageFactory : IMessageFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IActivityContextAccessor _activityContextAccessor;

        public MessageFactory([NotNull] IDateTimeProvider dateTimeProvider, IActivityContextAccessor activityContextAccessor)
        {
            _dateTimeProvider = dateTimeProvider;
            _activityContextAccessor = activityContextAccessor ?? throw new ArgumentNullException(nameof(activityContextAccessor));
        }

        /// <inheritdoc />
        public virtual Message<TKey, TValue> BuildMessage<TKey, TValue>([NotNull] IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            if (kafkaMessage == null) throw new ArgumentNullException(nameof(kafkaMessage));

            kafkaMessage.Headers.AddOrUpdate(AbstractionConstants.MessageTypeHeaderName, kafkaMessage.MessageType);
            kafkaMessage.Headers.AddOrUpdate(AbstractionConstants.MessageIdHeaderName, kafkaMessage.MessageId);

            kafkaMessage.Headers.AddOrUpdate(AbstractionConstants.CorrelationIdHeaderName, Activity.Current.Id);

            foreach (var baggageItem in _activityContextAccessor.Baggage)
            {
                kafkaMessage.Headers.AddOrUpdate(baggageItem.Key, baggageItem.Value);
            }

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