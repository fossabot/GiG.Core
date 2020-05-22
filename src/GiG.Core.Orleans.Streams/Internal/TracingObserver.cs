using OpenTelemetry.Trace;
using Orleans.Runtime;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DistributedTracingConstants = GiG.Core.DistributedTracing.Abstractions.Constants;

[assembly: InternalsVisibleTo("GiG.Core.Orleans.Tests.Unit")]
namespace GiG.Core.Orleans.Streams.Internal
{
    internal class TracingObserver<T> : IAsyncObserver<T>
    {
        private readonly IAsyncObserver<T> _observer;
        private readonly Tracer _tracer;

        public TracingObserver(IAsyncObserver<T> observer, Tracer tracer = null) 
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
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

            if (RequestContext.Get(DistributedTracingConstants.BaggageHeader) is string traceId)
            {
                consumerActivity.SetParentId(traceId);
            }
    
            if (RequestContext.Get(DistributedTracingConstants.BaggageHeader) is IEnumerable<KeyValuePair<string, string>> baggage)
            {
                foreach (var x in baggage)
                {
                    consumerActivity.AddBaggage(x.Key, x.Value);
                }
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
