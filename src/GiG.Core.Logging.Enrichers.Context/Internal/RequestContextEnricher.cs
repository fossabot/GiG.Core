using GiG.Core.Request.Abstractions;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Logging.Enrichers.Context.Internal
{
    internal class RequestContextEnricher : ILogEventEnricher
    {
        private const string IPAddress = "IPAddress";

        private readonly IRequestContextAccessor _requestContextAccessor;

        public RequestContextEnricher(IRequestContextAccessor requestContextAccessor)
        {
            _requestContextAccessor = requestContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var logEventProperty = new LogEventProperty(IPAddress, new ScalarValue(_requestContextAccessor?.IPAddress?.ToString()));

            logEvent.AddPropertyIfAbsent(logEventProperty);
        }
    }
}
