using System.Linq;
using GiG.Core.MultiTenant.Abstractions;
using Serilog.Core;
using Serilog.Events;

namespace GiG.Core.Logging.Enrichers.MultiTenant.Internal
{
    internal class TenantEnricher : ILogEventEnricher
    {
        private const string TenantId = "TenantId";

        private readonly ITenantAccessor _tenantAccessor;

        public TenantEnricher(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (_tenantAccessor.Values?.Any() ?? false)
            {
                var logEventProperty = propertyFactory.CreateProperty(TenantId, _tenantAccessor.Values, true);

                logEvent.AddPropertyIfAbsent(logEventProperty);
            }
        }
    }
}