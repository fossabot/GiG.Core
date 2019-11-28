using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Producers;
using Microsoft.Extensions.Logging;
using System;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class ProducerFactory : IProducerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessageFactory _messageFactory;

        /// <inheritdoc />
        public ProducerFactory(ILoggerFactory loggerFactory, IMessageFactory messageFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
        }

        /// <inheritdoc />
        public IKafkaProducer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var options = new KafkaBuilderOptions<TKey, TValue>(_messageFactory);
            setupAction?.Invoke(options);

            return new KafkaProducer<TKey, TValue>(options, _loggerFactory.CreateLogger<KafkaProducer<TKey, TValue>>());
        }
    }
}