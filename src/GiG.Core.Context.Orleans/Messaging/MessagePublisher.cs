using GiG.Core.Context.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Context.Orleans.Messaging
{
    /// <inheritdoc />
    public class MessagePublisher<T> : IMessagePublisher<T> where T : class
    {
        private IAsyncStream<T> _asyncStream;
        private ILogger _logger;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        /// <param name="logger"></param>
        public MessagePublisher(ILogger<MessagePublisher<T>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task PublishEventAsync([NotNull] T message, StreamSequenceToken token = null)
        {
            if (_asyncStream == null) throw new NullReferenceException(nameof(_asyncStream));

            //is this publish is a continuation of an existing flow we keep the same activity id
            if (RequestContext.ActivityId == Guid.Empty) RequestContext.ActivityId = Guid.NewGuid();
            RequestContext.PropagateActivityId = true;

            _logger.LogInformation($"Correlation Id before publish {RequestContext.ActivityId}");
            await _asyncStream.OnNextAsync(message, token);
        }

        /// <inheritdoc />
        public void SetAsyncStream([NotNull] IAsyncStream<T> asyncStream)
        {
            if (asyncStream == null) throw new ArgumentNullException(nameof(asyncStream));
            if (_asyncStream != null) throw new InvalidOperationException("Stream already set.");
            _asyncStream = asyncStream;
        }        
    }
}