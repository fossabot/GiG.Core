using GiG.Core.DistributedTracing.Abstractions.CorrelationId;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Logging.Enrichers.DistributedTracing
{
    internal class CorrelationIdEnricher : ILogEventEnricher
    {
        private const string CorrelationId = "CorrelationId";
     
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationIdEnricher(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logEventProperty = new LogEventProperty(CorrelationId, new ScalarValue(_correlationContextAccessor.Value.ToString()));

            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}
