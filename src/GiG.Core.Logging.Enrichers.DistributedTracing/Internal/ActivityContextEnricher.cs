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
            AddBaggage(logEvent);
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