using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Producers;
using Microsoft.Extensions.Logging;
using System;

namespace GiG.Core.Messaging.Kafka.Factories
{
    public class ProducerFactory : IProducerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessageFactory _messageFactory;

        public ProducerFactory(ILoggerFactory loggerFactory, IMessageFactory messageFactory)
        {
            _loggerFactory = loggerFactory; //Guard.IsNotNull(loggerFactory, nameof(loggerFactory));
            _messageFactory = messageFactory; //Guard.IsNotNull(messageFactory, nameof(messageFactory));
        }

        public IKafkaProducer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var options = new KafkaBuilderOptions<TKey, TValue>(_messageFactory);
            setupAction?.Invoke(options);

            return new KafkaProducer<TKey, TValue>(options, _loggerFactory.CreateLogger<KafkaProducer<TKey, TValue>>());
        }
    }
}