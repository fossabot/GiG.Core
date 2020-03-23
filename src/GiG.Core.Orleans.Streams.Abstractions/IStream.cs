using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// An abstraction for a stream publisher.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IStream<TMessage>
    {
        /// <summary>
        /// Publishes a message using the underlying provider from which the stream publisher was created.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="token">Handle representing stream sequence number/token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task PublishAsync(TMessage message, StreamSequenceToken token = null);

        /// <summary>
        /// Subscribes a consumer to this observable.
        /// </summary>
        /// <param name="observer">The <see cref="IAsyncObserver{T}"/> to subscribe.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"/> to be used as an offset to start the subscription from.</param>
        /// <returns>A promise for a StreamSubscriptionHandle that represents the subscription. The
        ///     consumer may unsubscribe by using this handle. The subscription remains active
        ///     for as long as it is not explicitly unsubscribed.
        ///</returns>
        Task<StreamSubscriptionHandle<TMessage>> SubscribeAsync(IAsyncObserver<TMessage> observer, StreamSequenceToken token = null);
    }
}