using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Consumers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class ConsumerFactory : IConsumerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        /// <inheritdoc />
        public ConsumerFactory([NotNull] ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <inheritdoc />
        public IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var builderOptions = new KafkaBuilderOptions<TKey, TValue>();
            setupAction?.Invoke(builderOptions);

            return new KafkaConsumer<TKey, TValue>(builderOptions, _loggerFactory.CreateLogger<KafkaConsumer<TKey, TValue>>());
        }
    }
}
