using GiG.Core.DistributedTracing.Abstractions;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Logging.Enrichers.DistributedTracing.Internal
{
    /// <summary>
    /// Enriches Log Events with field acquired from the Activity Context.
    /// </summary>
    internal class ActivityContextEnricher : ILogEventEnricher
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="activityContextAccessor">An implementation of <see cref="IActivityContextAccessor"/> to retrieve trace information from.</param>
        public ActivityContextEnricher(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }
        
        /// <inheritdoc />
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddCorrelationId(logEvent);
            AddTraceId(logEvent);
            AddSpanId(logEvent);
            AddParentId(logEvent);
            AddBaggage(logEvent);
        }

        private void AddCorrelationId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TracingFields.CorrelationId, new ScalarValue(_activityContextAccessor.CorrelationId)));
        }

        private void AddTraceId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TracingFields.TraceId, new ScalarValue(_activityContextAccessor.TraceId)));
        }

        private void AddSpanId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TracingFields.SpanId, new ScalarValue(_activityContextAccessor.SpanId)));
        }

        private void AddParentId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty(TracingFields.ParentId, new ScalarValue(_activityContextAccessor.ParentSpanId)));
        }
        
        private void AddBaggage(LogEvent logEvent)
        {
            foreach (var baggageEntry in _activityContextAccessor.Baggage)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty($"{TracingFields.BaggagePrefix}{baggageEntry.Key}", new ScalarValue(baggageEntry.Value)));
            }
        }
    }
}