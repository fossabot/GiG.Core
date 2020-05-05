﻿using Confluent.Kafka;
 using System.Diagnostics;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Provides a factory to create an instance of <see cref="Message{TKey,TValue}"/>.
    /// </summary>
    public interface IMessageFactory
    {
        /// <summary>
        /// Build a kafka message.
        /// </summary>
        /// <param name="kafkaMessage">The actual Kafka message.</param>
        /// <typeparam name="TKey">The Key of the Kafka message.</typeparam>
        /// <typeparam name="TValue">The Value of the Kafka message.</typeparam>
        /// <param name="publishingActivity">The <see cref="Activity"/>.</param>
        /// <returns>The <see cref="Message{TKey, TValue}"/>.</returns>
        Message<TKey, TValue> BuildMessage<TKey, TValue>(IKafkaMessage<TKey, TValue> kafkaMessage, Activity publishingActivity);
    }
}