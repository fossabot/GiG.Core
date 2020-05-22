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
            var activity = CreateActivity($"{Constants.ConsumeActivityName}-{nameof(OnErrorAsync)}");
            activity.Start();

            var span = _tracer?.StartSpanFromActivity($"{Constants.SpanConsumeOperationNamePrefix}-{nameof(OnErrorAsync)}", activity, SpanKind.Internal);

            await _observer.OnErrorAsync(ex);
            
            activity.Stop();
            span?.End();
        }

        public async Task OnNextAsync(T item, StreamSequenceToken token = null)
        {
            var activity = CreateActivity(Constants.ConsumeActivityName);
            activity.Start();

            var span = _tracer?.StartSpanFromActivity($"{Constants.SpanConsumeOperationNamePrefix}-{item.GetType().Name}", activity, SpanKind.Consumer);

            await _observer.OnNextAsync(item, token);

            activity.Stop();
            span?.End();
        }
        
        private static Activity CreateActivity(string operationName)
        {
            var activity = new Activity(operationName);

            if (RequestContext.Get(DistributedTracingConstants.BaggageHeader) is string traceId)
            {
                activity.SetParentId(traceId);
            }
    
            if (RequestContext.Get(DistributedTracingConstants.BaggageHeader) is IEnumerable<KeyValuePair<string, string>> baggage)
            {
                foreach (var x in baggage)
                {
                    activity.AddBaggage(x.Key, x.Value);
                }
            }

            return activity;
        }
    }
}
