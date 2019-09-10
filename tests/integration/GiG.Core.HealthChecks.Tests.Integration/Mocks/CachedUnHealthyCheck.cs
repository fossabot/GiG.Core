using System.Threading;
using System.Threading.Tasks;
using GiG.Core.HealthChecks.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal class CachedUnHealthyCheck : CachedHealthCheck
    {
        public CachedUnHealthyCheck(IMemoryCache memoryCache) : base(memoryCache)
        {
        }

        protected override Task<HealthCheckResult> CheckCachedHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}