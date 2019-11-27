﻿using GiG.Core.Messaging.Kafka.Abstractions.Extensions;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IKafkaBuilderOptions<TKey, TValue>
    {
        KafkaProviderOptions KafkaProviderOptions { get; set; }

        Serializers<TKey, TValue> Serializers { get; set; }

        IMessageFactory MessageFactory { get; set; }
    }
}