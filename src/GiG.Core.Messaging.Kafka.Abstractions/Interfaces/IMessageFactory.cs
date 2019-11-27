﻿using Confluent.Kafka;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IMessageFactory
    {
        Message<TKey, TValue> BuildMessage<TKey, TValue>(IKafkaMessage<TKey, TValue> kafkaMessage);
    }
}