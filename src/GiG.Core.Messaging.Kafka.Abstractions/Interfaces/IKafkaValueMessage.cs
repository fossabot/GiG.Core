﻿namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Kafka value message.
    /// </summary>
    /// <typeparam name="T">Generic to define the type of message.</typeparam>
    public interface IKafkaValueMessage<T>
    {
        /// <summary>
        /// The unique Resource id of the message.
        /// </summary>
        string ResourceId { get; set; }
        
        /// <summary>
        /// Previous data.
        /// </summary>
        T PreviousData { get; set; }
        
        /// <summary>
        /// Current data.
        /// </summary>
        T CurrentData { get; set; }
    }
}