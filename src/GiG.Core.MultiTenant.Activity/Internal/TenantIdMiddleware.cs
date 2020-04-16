using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.MultiTenant.Activity.Internal
{
    internal class TenantIdMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // If Activity is not populated, skip
            var current = System.Diagnostics.Activity.Current;
            if (current == null)
            {
                await _next(context);
                return;
            }

            // If Tenant already exists in Baggage, skip
            var tenantExistsInBaggage = current.Baggage.Any(x => x.Key == Constants.TenantIdBaggageKey);
            if (tenantExistsInBaggage)
            {
                await _next(context);
                return;
            }

            // If Tenant exists in Header add it to Baggage
            context.Request?.Headers?.TryGetValue(Constants.Header, out var values);
            foreach (var tenant in values)
            {
                current.AddBaggage(Constants.TenantIdBaggageKey, tenant);
            }

            await _next(context);
        }
    }
}