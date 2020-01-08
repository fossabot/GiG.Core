using System;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Kafka producer configured by a key-value pair.
    /// </summary>
    /// <typeparam name="TKey">The Key of the Message.</typeparam>
    /// <typeparam name="TValue">The Value of the Message.</typeparam>
    public interface IKafkaProducer<TKey, TValue> : IDisposable
    {
        /// <summary>
        ///  Asynchronously send a single message to a Kafka topic/partition.
        /// </summary>
        /// <param name="kafkaMessage">The <see cref="IKafkaMessage{TKey, TValue}"/>.</param>
        /// <returns></returns>
        Task ProduceAsync(IKafkaMessage<TKey, TValue> kafkaMessage);
    }
}