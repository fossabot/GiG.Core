using GiG.Core.DistributedTracing.Abstractions.CorrelationId;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using System;

namespace GiG.Core.Extensions.Logging.Enrichers
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
