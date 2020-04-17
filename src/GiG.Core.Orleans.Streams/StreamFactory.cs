using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using Orleans.Streams;
using System;
using System.IO;

namespace GiG.Core.Orleans.Streams
{

    /// <summary>
    /// Stream Factory for Orleans.
    /// </summary>
    public class StreamFactory : IStreamFactory
    {
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly TracerFactory _tracerFactory;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="activityContextAccessor">The <see cref="IActivityContextAccessor" /> to use to add activity id within RequestContext.</param>
        /// <param name="tracerFactory">The <see cref="TracerFactory"/> used to get the <see cref="Tracer"/> used for Telemetry.</param>
        public StreamFactory(IActivityContextAccessor activityContextAccessor, TracerFactory tracerFactory = null)
        {
            _activityContextAccessor = activityContextAccessor;
            _tracerFactory = tracerFactory;
        }

        /// <inheritdoc />
        public IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string streamNameSpace)
        {
            var stream = streamProvider.GetStream<TMessage>(streamId, streamNameSpace);
          
            return new Stream<TMessage>(stream, _activityContextAccessor, _tracerFactory);
        }
        
        /// <inheritdoc />
        public IStream<TMessage> GetStream<TMessage>(IStreamProvider streamProvider, Guid streamId, string domain, string streamType, uint? version)
        {
            var stream =
                streamProvider.GetStream<TMessage>(streamId, StreamHelper.GetNamespace(domain, streamType, version));
          
            return new Stream<TMessage>(stream, _activityContextAccessor, _tracerFactory);
        }
    }
}