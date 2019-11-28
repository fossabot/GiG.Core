using System;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Provides a factory to create an instance of <see cref="IKafkaConsumer{TKey, TValue}"/>.
    /// </summary>
    public interface IConsumerFactory
    {
        /// <summary>
        /// Returns a Kafka consumer from the passed setup.
        /// </summary>
        /// <param name="setupAction">The <see><cref>Action{KafkaBuilderOptions{TKey, TValue}}</cref></see> which will be used to set kafka builder options.</param>
        /// <typeparam name="TKey">The Key of the Kafka message.</typeparam>
        /// <typeparam name="TValue">The Value of the Kafka message.</typeparam>
        /// <returns>The <see cref="IKafkaConsumer{TKey, TValue}"/>.</returns>
        IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction);
    }
}