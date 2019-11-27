using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IProducerFactory
    {
        IKafkaProducer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction);
    }
}