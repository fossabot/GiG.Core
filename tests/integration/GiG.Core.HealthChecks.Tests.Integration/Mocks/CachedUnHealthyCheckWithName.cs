using GiG.Core.HealthChecks.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal class CachedUnHealthyCheckWithName : CachedHealthCheck
    {
        public CachedUnHealthyCheckWithName(IMemoryCache memoryCache) : base(memoryCache, nameof(CachedUnHealthyCheckWithName))
        {
        }

        protected override Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}