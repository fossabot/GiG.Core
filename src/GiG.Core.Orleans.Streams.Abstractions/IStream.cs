using Orleans.Streams;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// An abstraction for a stream publisher.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IStream<in TMessage>
    {
        /// <summary>
        /// Publishes a message using the underlying provider from which the stream publisher was created.
        /// </summary>
        /// <param name="message">The message to publish.</param>
        /// <param name="token">Handle representing stream sequence number/token.</param>
        /// <returns>An awaitable <see cref="Task"/>.</returns>
        Task PublishAsync(TMessage message, StreamSequenceToken token = null);
    }
}