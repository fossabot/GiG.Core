using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams
{
    /// <summary>
    /// Stream to publish Event Messages over Orleans Streams.
    /// </summary>
    /// <typeparam name="TMessage">Stream Message.</typeparam>
    public class Stream<TMessage> : IStream<TMessage>
    {
        private readonly IAsyncStream<TMessage> _asyncStream;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asyncStream">The <see cref="IAsyncStream{TMessage}"/> to be used in the Stream Publisher./></param>
        /// <param name="correlationContextAccessor">The <see cref="ICorrelationContextAccessor" /> to use to add the Correlation ID within RequestContext.</param>
        public Stream(IAsyncStream<TMessage> asyncStream, ICorrelationContextAccessor correlationContextAccessor)
        {
            _asyncStream = asyncStream ?? throw new ArgumentNullException(nameof(asyncStream));
            _correlationContextAccessor = correlationContextAccessor ?? throw new ArgumentNullException(nameof(correlationContextAccessor));
        }

        /// <summary>
        /// Used to publish the message using the underlying stream. Before sending the message the correlation id is set if not already present.
        /// </summary>
        /// <param name="message">The Message to be published.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"/> to send with the Message.</param>
        /// <returns></returns>
        public async Task PublishAsync(TMessage message, StreamSequenceToken token = null)
        {
            var correlationId = _correlationContextAccessor.Value;

            // This is to ensure that the correlation id provided by the accessor is propagated in the orleans request context.
            if (correlationId != RequestContext.ActivityId)
            {
                RequestContext.ActivityId = correlationId;
            }
          
            await _asyncStream.OnNextAsync(message, token);
        }
    }
}