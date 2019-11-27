﻿using System.Collections.Generic;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    public interface IKafkaMessage<TKey, TValue>
    {
        TKey Key { get; set; }

        TValue Value { get; set; }

        string MessageType { get; set; }

        string MessageId { get; set; }

        IDictionary<string, string> Headers { get; set; }
    }
}