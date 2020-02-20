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
    public class Stream<TMessage> : IStream<TMessage> where TMessage : IMessage
    {
        private readonly IAsyncStream<TMessage> _asyncStream;
        private readonly IActivityContextAccessor _activityContextAccessor;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asyncStream">The <see cref="IAsyncStream{TMessage}"/> to be used in the Stream Publisher./></param>
        /// <param name="activityContextAccessor">The <see cref="IActivityContextAccessor" /> to use to add the Activity ID within RequestContext.</param>
        public Stream(IAsyncStream<TMessage> asyncStream, IActivityContextAccessor activityContextAccessor)
        {
            _asyncStream = asyncStream ?? throw new ArgumentNullException(nameof(asyncStream));
            _activityContextAccessor = activityContextAccessor;
        }

        /// <summary>
        /// Used to publish the message using the underlying stream. Before sending the message the activity id is set if not already present.
        /// </summary>
        /// <param name="message">The Message to be published.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"/> to send with the Message.</param>
        /// <returns></returns>
        public async Task PublishAsync(TMessage message, StreamSequenceToken token = null)
        {
            var activityId = _activityContextAccessor?.CorrelationId;
            // This is to ensure that the correlation id provided by the accessor is propagated in the orleans request context.
            if (!string.IsNullOrWhiteSpace(activityId))
            {
                RequestContext.Set(Constants.ActivityHeader, activityId);
            }
          
            await _asyncStream.OnNextAsync(message, token);
        }
    }
}