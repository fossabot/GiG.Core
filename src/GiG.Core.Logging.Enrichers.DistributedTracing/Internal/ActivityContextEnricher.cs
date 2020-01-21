using GiG.Core.DistributedTracing.Abstractions;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Logging.Enrichers.DistributedTracing.Internal
{
    internal class ActivityContextEnricher : ILogEventEnricher
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        public ActivityContextEnricher(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            AddTraceId(logEvent);
            AddSpanId(logEvent);
            AddParentId(logEvent);
            AddBaggage(logEvent);
        }

        private void AddTraceId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("TraceId", new ScalarValue(_activityContextAccessor.TraceId)));
        }

        private void AddSpanId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("SpanId", new ScalarValue(_activityContextAccessor.SpanId)));
        }

        private void AddParentId(LogEvent logEvent)
        {
            logEvent.AddPropertyIfAbsent(new LogEventProperty("ParentId", new ScalarValue(_activityContextAccessor.ParentSpanId)));
        }
        
        private void AddBaggage(LogEvent logEvent)
        {
            foreach (var baggageEntry in _activityContextAccessor.Baggage)
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty($"baggage.{baggageEntry.Key}", new ScalarValue(baggageEntry.Value)));
            }
        }
    }
}