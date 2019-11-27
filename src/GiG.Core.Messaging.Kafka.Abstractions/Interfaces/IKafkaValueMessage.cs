﻿namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IKafkaValueMessage<T>
    {
        string ResourceId { get; set; }
        T PreviousData { get; set; }
        T CurrentData { get; set; }
    }
}