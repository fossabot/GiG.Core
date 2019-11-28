using Confluent.Kafka;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Kafka builder options.
    /// </summary>
    /// <typeparam name="TKey">The key of the kafka message.</typeparam>
    /// <typeparam name="TValue">The value of the kafka message.</typeparam>
    public interface IKafkaBuilderOptions<TKey, TValue>
    {
        /// <summary>
        /// Kafka Provider Options.
        /// </summary>
        KafkaProviderOptions KafkaProviderOptions { get; set; }

        /// <summary>
        /// Serializers.
        /// </summary>
        Serializers<TKey, TValue> Serializers { get; set; }

        /// <summary>
        /// Provides a factory to create an instance of <see cref="Message{TKey,TValue}"/>.
        /// </summary>
        IMessageFactory MessageFactory { get; set; }
    }
}