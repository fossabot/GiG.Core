using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Streams.Abstractions;
using GiG.Core.Orleans.Streams.Internal;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Diagnostics;
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
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly Tracer _tracer;

        private const string TracerName = "StreamTracer";
        private const string SpanOperationNamePrefix = "StreamPublisher";

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="asyncStream">The <see cref="IAsyncStream{TMessage}"/> to be used in the Stream Publisher./></param>
        /// <param name="activityContextAccessor">The <see cref="IActivityContextAccessor" /> to use to add the Activity ID within RequestContext.</param>
        /// <param name="tracerFactory">The <see cref="TracerFactory"/> used to get the <see cref="Tracer"/> used for Telemetry.</param>
        public Stream(IAsyncStream<TMessage> asyncStream, IActivityContextAccessor activityContextAccessor,
            TracerFactory tracerFactory = null)
        {
            _asyncStream = asyncStream ?? throw new ArgumentNullException(nameof(asyncStream));
            _activityContextAccessor = activityContextAccessor;
            _tracer = tracerFactory?.GetTracer(TracerName);
        }

        /// <summary>
        /// Used to publish the message using the underlying stream. Before sending the message the activity id is set if not already present.
        /// </summary>
        /// <param name="message">The Message to be published.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"/> to send with the Message.</param>
        /// <returns></returns>
        public async Task PublishAsync(TMessage message, StreamSequenceToken token = null)
        {
            var publishingActivity = new Activity("Stream Publish");
            publishingActivity.Start();

            var span = _tracer?.StartSpanFromActivity($"{SpanOperationNamePrefix}-{message.GetType().Name}", Activity.Current, SpanKind.Producer);

            RequestContext.Set(Constants.ActivityHeader, publishingActivity.Id);

            await _asyncStream.OnNextAsync(message, token);

            span?.End();
            publishingActivity.Stop();
        }

        /// <summary>
        /// Subscribes a consumer to this observable, adding tracing in the consumption of items.
        /// </summary>
        /// <param name="observer">The <see cref="IAsyncObserver{T}"/> to subscribe.</param>
        /// <param name="token">The <see cref="StreamSequenceToken"/> to be used as an offset to start the subscription from.</param>
        /// <returns>A promise for a StreamSubscriptionHandle that represents the subscription. The
        ///     consumer may unsubscribe by using this handle. The subscription remains active
        ///     for as long as it is not explicitly unsubscribed.
        ///</returns>
        public async Task<StreamSubscriptionHandle<TMessage>> SubscribeAsync(IAsyncObserver<TMessage> observer, StreamSequenceToken token)
        {
            var tracingObserver = new TracingObserver<TMessage>(observer, _activityContextAccessor, _tracer);
            return await _asyncStream.SubscribeAsync(tracingObserver, token);
        }
    }
}