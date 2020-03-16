using GiG.Core.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
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
            var tenantIds = context.Request?.Headers?.TryGetValue(Constants.Header, out var value) != true
                ? new List<string>()
                : value.Where(x => !string.IsNullOrWhiteSpace(x));
            
            foreach (var tenant in tenantIds)
                Activity.Current.AddBaggage(Constants.TenantIdBaggageKey, tenant);
            
            await _next(context);
        }
    }
}