﻿using System.Collections.Generic;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Represents a Kafka message.
    /// </summary>
    /// <typeparam name="TKey">The message key value.</typeparam>
    /// <typeparam name="TValue">The message value.</typeparam>
    public interface IKafkaMessage<TKey, TValue>
    {
        /// <summary>
        /// The message key value (possibly null).
        /// </summary>
        TKey Key { get; set; }

        /// <summary>
        /// The message value (possibly null).
        /// </summary>
        TValue Value { get; set; }

        /// <summary>
        /// The message type.
        /// </summary>
        string MessageType { get; set; }

        /// <summary>
        /// The unique message id.
        /// </summary>
        string MessageId { get; set; }

        /// <summary>
        /// A <see><cref>IDictionary{string, string}</cref></see> of Headers to be used.
        /// </summary>
        IDictionary<string, string> Headers { get; set; }
    }
}