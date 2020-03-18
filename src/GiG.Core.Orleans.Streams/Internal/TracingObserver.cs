using GiG.Core.DistributedTracing.Abstractions;
using OpenTelemetry.Trace;
using Orleans.Streams;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Streams.Internal
{
    internal class TracingObserver<T> : IAsyncObserver<T>
    {
        private readonly IAsyncObserver<T> _observer;
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly Tracer _tracer;

        public TracingObserver(IAsyncObserver<T> observer, IActivityContextAccessor activityContextAccessor, Tracer tracer = null) 
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _activityContextAccessor = activityContextAccessor ?? throw new ArgumentNullException(nameof(activityContextAccessor));
            _tracer = tracer; 
        }

        public async Task OnCompletedAsync()
        {
            await _observer.OnCompletedAsync();
        }

        public async Task OnErrorAsync(Exception ex)
        {
            await _observer.OnErrorAsync(ex);
        }

        public async Task OnNextAsync(T item, StreamSequenceToken token = null)
        {
            var itemTypeName = item.GetType().Name;
            var consumerActivity = new Activity("TracingObserver-Consuming-Item");
            consumerActivity.Start();

            var span = _tracer?.StartSpanFromActivity($"Consuming-{itemTypeName}", consumerActivity, SpanKind.Consumer);

            span?.AddEvent($"Calling {_observer.GetType().Name} OnNextAsync");
            
            await _observer.OnNextAsync(item, token);

            span?.End();
            consumerActivity.Stop();
        }
    }
}
