using GiG.Core.DistributedTracing.Abstractions;
using OpenTelemetry.Trace;
using Orleans.Streams;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("GiG.Core.Orleans.Tests.Unit")]
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
            var consumerActivity = new Activity(Constants.ConsumeActivityName);

            consumerActivity.SetParentId(_activityContextAccessor.ParentId);
            
            foreach (var baggage in _activityContextAccessor.Baggage)
            {
                consumerActivity.AddBaggage(baggage.Key, baggage.Value);
            }
            
            consumerActivity.Start();

            var span = _tracer?.StartSpanFromActivity($"{Constants.SpanConsumeOperationNamePrefix}-{item.GetType().Name}", 
                consumerActivity, SpanKind.Consumer);

            await _observer.OnNextAsync(item, token);

            consumerActivity.Stop();
            span?.End();
        }
    }
}
