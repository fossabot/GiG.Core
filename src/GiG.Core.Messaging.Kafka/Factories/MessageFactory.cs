using Confluent.Kafka;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.Providers.DateTime.Abstractions;
using JetBrains.Annotations;
using System;
using System.Linq;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class MessageFactory : IMessageFactory
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        
        public MessageFactory([NotNull] IDateTimeProvider dateTimeProvider, ITenantAccessor tenantAccessor = null, ICorrelationContextAccessor correlationContextAccessor = null)
        {
            _dateTimeProvider = dateTimeProvider;
            _tenantAccessor = tenantAccessor;
            _correlationContextAccessor = correlationContextAccessor;
        }
        
        /// <inheritdoc />
        public virtual Message<TKey, TValue> BuildMessage<TKey, TValue>([NotNull] IKafkaMessage<TKey, TValue> kafkaMessage)
        {
            if (kafkaMessage == null) throw new ArgumentNullException(nameof(kafkaMessage));
            
            kafkaMessage.Headers.AddOrUpdate(Constants.MessageTypeHeaderName, kafkaMessage.MessageType);
            kafkaMessage.Headers.AddOrUpdate(Constants.MessageIdHeaderName, kafkaMessage.MessageId);

            if (_tenantAccessor != null)
            {
                var tenants = string.Join(",", _tenantAccessor.Values.ToArray());
                kafkaMessage.Headers.AddOrUpdate(MultiTenant.Abstractions.Constants.Header, tenants);
            }

            if (_correlationContextAccessor != null)
            {
                kafkaMessage.Headers.AddOrUpdate(DistributedTracing.Abstractions.Constants.Header, _correlationContextAccessor.Value.ToString());
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