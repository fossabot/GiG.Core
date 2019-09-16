using GiG.Core.HealthChecks.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Web.Sample.HealthChecks
{
    public class DummyCachedHealthCheck : CachedHealthCheck
    {
        private readonly ILogger _logger;

        public DummyCachedHealthCheck(ILogger<DummyCachedHealthCheck> logger, IMemoryCache memoryCache) : base(memoryCache)
        {
            _logger = logger;
        }

        protected override Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Dummy Health Check");

            return Task.FromResult(HealthCheckResult.Healthy());
        }
    }
}
