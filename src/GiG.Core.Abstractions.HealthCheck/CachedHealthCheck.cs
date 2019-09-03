using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Abstractions.HealthCheck
{
    public abstract class CachedHealthCheck : IHealthCheck
    {
        private readonly IMemoryCache _memoryCache;
        private readonly long _cacheExpirationMS;
        private readonly string _cacheKey;

        protected CachedHealthCheck(IMemoryCache memoryCache, long cacheExpirartionMs = 2000)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMS = cacheExpirartionMs;
            _cacheKey = GetType().FullName;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(_cacheKey, async result =>
            {
                result.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(_cacheExpirationMS);
                var res = await DoHealthCheckAsync(context, cancellationToken);
                return res;
            });
        }

        protected abstract Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken = default);
    }
}