using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using Orleans.Streams;
using System;

namespace GiG.Core.Orleans.Streams
{

    /// <summary>
    /// Steam Factory for Orleans
    /// </summary>
    public class StreamFactory : IStreamFactory
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="correlationContextAccessor">The <see cref="ICorrelationContextAccessor" /> to use to add correlationId within RequestContext.</param>
        public StreamFactory(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        /// <summary>
        /// Get instance of stream.
        /// </summary>
        /// <param name="streamProvider">The <see cref="IStreamProvider"/> provider for streaming.</param>
        /// <param name="streamId">The stream identifier.</param>
        /// <param name="streamNameSpace">The stream namespace.</param>
        /// <typeparam name="TMessage">Stream Message.</typeparam>
        /// <returns>The <see cref="IStream{TMessage}"/> stream. </returns>
        public IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace)
        {
            var stream = streamProvider.GetStream<TMessage>(streamId, streamNameSpace);
          
            return new Stream<TMessage>(stream, _correlationContextAccessor);
        }
    }
}
