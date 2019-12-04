using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Provides a factory to create an instance of <see cref="IKafkaProducer{TKey, TValue}"/>.
    /// </summary>
    public interface IProducerFactory
    {
        /// <summary>
        /// Returns a Kafka producer from the passed setup.
        /// </summary>
        /// <param name="setupAction">The <see><cref>Action{KafkaBuilderOptions{TKey, TValue}}</cref></see> which will be used to set kafka builder options.</param>
        /// <typeparam name="TKey">The Key of the Kafka message.</typeparam>
        /// <typeparam name="TValue">The Value of the Kafka message.</typeparam>
        /// <returns>The <see cref="IKafkaProducer{TKey, TValue}"/>.</returns>
        IKafkaProducer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction);
    }
}