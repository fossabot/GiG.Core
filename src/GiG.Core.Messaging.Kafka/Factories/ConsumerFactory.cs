using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Consumers;
using Microsoft.Extensions.Logging;
using System;

namespace GiG.Core.Messaging.Kafka.Factories
{
    public class ConsumerFactory : IConsumerFactory
    {
        private readonly ILoggerFactory _loggerFactory;

        public ConsumerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;  //Guard.IsNotNull(loggerFactory, nameof(loggerFactory));
        }

        public IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var builderOptions = new KafkaBuilderOptions<TKey, TValue>();
            setupAction?.Invoke(builderOptions);

            return new KafkaConsumer<TKey, TValue>(builderOptions, _loggerFactory.CreateLogger<KafkaConsumer<TKey, TValue>>());
        }
    }
}
