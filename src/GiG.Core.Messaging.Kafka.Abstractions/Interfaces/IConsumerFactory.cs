using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IConsumerFactory
    {
        IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction);
    }
}