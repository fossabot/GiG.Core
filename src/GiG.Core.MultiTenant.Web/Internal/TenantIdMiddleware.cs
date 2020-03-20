using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.MultiTenant.Web.Internal
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
            var tenantExistsInBaggage = Activity.Current?.Baggage.Any(x => x.Key == "TenantId");

            if (tenantExistsInBaggage.HasValue && tenantExistsInBaggage.Value)
                await _next(context);

            context.Request?.Headers?.TryGetValue(Constants.Header, out var values);

            var tenantIds = values.ToString()
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(m => m.Trim())
                .ToList();

            if (tenantIds.Any())
            {
                foreach (var tenant in tenantIds)
                    Activity.Current.AddBaggage(Constants.TenantIdBaggageKey, tenant);
            }

            await _next(context);
        }
    }
}