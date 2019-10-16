using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Context.Abstractions
{
    /// <summary>
    /// Message Publisher used to publish event messages over Orleans Streams from Grains.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMessagePublisher<T>
    {
        /// <summary>
        /// Used to publish the message using the underlying stream. Before send the message the correlation id is set if not already present
        /// </summary>
        /// <param name="message">The message to send.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"> to send with the message</see></param>
        /// <returns></returns>
        Task PublishEventAsync(T message, StreamSequenceToken token = null);

        /// <summary>
        /// Sets the async stream which is used by the publisher. Generally this is set in the OnActivateAsync of the grain.
        /// </summary>
        /// <param name="asyncStream">The <see cref="IAsyncStream{T}"/> to use in the publisher./></param>
        void SetAsyncStream(IAsyncStream<T> asyncStream);
    }
}