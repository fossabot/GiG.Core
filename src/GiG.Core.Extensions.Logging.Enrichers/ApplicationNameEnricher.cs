using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Extensions.Logging.Enrichers
{
    internal class ApplicationNameEnricher : ILogEventEnricher
    {
        private const string ApplicationName = "ApplicationName";

        private readonly LogEventProperty _logEventProperty;

        public ApplicationNameEnricher(IConfiguration configuration)
        {
            _logEventProperty =
                new LogEventProperty(ApplicationName, new ScalarValue(configuration[ApplicationName]));
        }
        
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(_logEventProperty);
        }
    }
}