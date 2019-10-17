using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using System;

namespace GiG.Core.Orleans.Streams
{

    public class StreamFactory : IStreamFactory
    {
        private readonly ILogger<StreamFactory> _logger;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public StreamFactory(ILogger<StreamFactory> logger, ICorrelationContextAccessor correlationContextAccessor)
        {
            _logger = logger;
            _correlationContextAccessor = correlationContextAccessor;
        }

        public IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace)
        {
            var stream = streamProvider.GetStream<TMessage>(streamId, streamNameSpace);
          
            return new Stream<TMessage>(stream, _logger, _correlationContextAccessor);
        }
    }
}
