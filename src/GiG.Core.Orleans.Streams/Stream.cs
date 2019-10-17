using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams
{
    public class Stream<TMessage> : IStream<TMessage>
    {
        private IAsyncStream<TMessage> _asyncStream;
        private readonly ILogger _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public Stream(IAsyncStream<TMessage> asyncStream, ILogger logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            _asyncStream = asyncStream;
            _logger = logger;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task PublishAsync([NotNull] TMessage message, StreamSequenceToken token = null)
        {
            if (_asyncStream == null) throw new NullReferenceException(nameof(_asyncStream));

            var correlatioId = _correlationContextAccessor.Value;

            //this is to ensure that the correlation id provided by the accessor is propagated in the orleans request context.
            if (correlatioId != RequestContext.ActivityId)
            {
                RequestContext.ActivityId = correlatioId;
            }

            _logger.LogInformation("Correlation Id before publish {ActivityId}", _correlationContextAccessor.Value);
            await _asyncStream.OnNextAsync(message, token);
        }
    }
}