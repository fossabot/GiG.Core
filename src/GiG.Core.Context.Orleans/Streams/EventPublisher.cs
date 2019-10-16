using JetBrains.Annotations;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Context.Orleans.Streams
{
    public class EventPublisher<T> : IEventPublisher<T> where T : EventMessageBase
    {
        private IAsyncStream<T> _asyncStream;

        public async Task PublishEventAsync([NotNull] T eventMessage, StreamSequenceToken token = null)
        {
            if (eventMessage == null) throw new ArgumentNullException(nameof(eventMessage));

            var headerKey = DistributedTracing.Abstractions.Constants.Header;
            var context = eventMessage.RequestContext = new Dictionary<string, string>();

            if (!context.ContainsKey(DistributedTracing.Abstractions.Constants.Header) ||
                context[headerKey] != string.Empty ||
                context[headerKey] != Guid.Empty.ToString())
            {
                eventMessage.RequestContext.Add(headerKey, Guid.NewGuid().ToString());
            }

            await _asyncStream.OnNextAsync(eventMessage, token);
        }

        public async Task PublishEventAsync([NotNull] T eventMessage, [NotNull] Dictionary<string, string> requestContext, 
                                            StreamSequenceToken token = null)
        {
            if (eventMessage == null) throw new ArgumentNullException(nameof(requestContext));
            if (requestContext == null) throw new ArgumentNullException(nameof(requestContext));
            
            eventMessage.RequestContext = requestContext;

            await _asyncStream.OnNextAsync(eventMessage, token);
        }

        public void SetAsyncStream([NotNull] IAsyncStream<T> asyncStream)
        {
            if (asyncStream == null) throw new ArgumentNullException(nameof(asyncStream));
            _asyncStream = asyncStream;
        }
    }
}