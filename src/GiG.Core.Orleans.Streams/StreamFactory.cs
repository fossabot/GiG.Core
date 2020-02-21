using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using Orleans.Streams;
using System;

namespace GiG.Core.Orleans.Streams
{

    /// <summary>
    /// Stream Factory for Orleans.
    /// </summary>
    public class StreamFactory : IStreamFactory
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activityContextAccessor">The <see cref="IActivityContextAccessor" /> to use to add activity id within RequestContext.</param>
        public StreamFactory(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }

        /// <summary>
        /// Returns an instance of <see cref="IStream{TMessage}"/>.
        /// </summary>
        /// <param name="streamProvider">The <see cref="IStreamProvider"/>.</param>
        /// <param name="streamId">The stream identifier.</param>
        /// <param name="streamNameSpace">The stream namespace.</param>
        /// <typeparam name="TMessage">Stream Message.</typeparam>
        /// <returns>The <see cref="IStream{TMessage}"/> stream. </returns>
        public IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace) where TMessage : IMessage
        {
            var stream = streamProvider.GetStream<TMessage>(streamId, streamNameSpace);
          
            return new Stream<TMessage>(stream, _activityContextAccessor);
        }
    }
}