using System;
using System.Threading;

namespace GiG.Core.Messaging.Kafka.Abstractions.Interfaces
{
    /// <summary>
    /// Kafka consumer configured by a key-value pair.
    /// </summary>
    /// <typeparam name="TKey">The Key of the Message.</typeparam>
    /// <typeparam name="TValue">The Value of the Message.</typeparam>
    public interface IKafkaConsumer<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// Poll for new messages / events. Blocks until a consume result is available or the operation has been cancelled.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        IKafkaMessage<TKey, TValue> Consume(CancellationToken cancellationToken = default);

        /// <summary>
        /// Commits an offset based on the topic/partition/offset of a ConsumeResult.
        /// </summary>
        /// <param name="message">The <see cref="IKafkaMessage{TKey, TValue}"/>.</param>
        void Commit(IKafkaMessage<TKey, TValue> message);
    }
}