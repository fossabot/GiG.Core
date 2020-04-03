using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Internal
{
    /// <summary>
    /// Base Abstract Class for Cached Health Checks.
    /// </summary>
    internal class CachedHealthCheck : IHealthCheck
    {
        internal const long DefaultCacheExpirationMs = 500;

        private readonly IHealthCheck _healthCheck;
        private readonly IMemoryCache _memoryCache;
        private readonly string _cacheKey;
        private readonly ulong _cacheExpirationMs;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="healthCheck">The Health Check.</param>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in milliseconds.</param>
        public CachedHealthCheck(IHealthCheck healthCheck, IMemoryCache memoryCache, ulong cacheExpirationMs = DefaultCacheExpirationMs)
        {
            _healthCheck = healthCheck;
            _memoryCache = memoryCache;
            _cacheKey = GetType().FullName + _healthCheck.GetType().FullName;
            _cacheExpirationMs = cacheExpirationMs;
        }

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(_cacheKey, async result =>
            {
                result.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(_cacheExpirationMs);

                return await _healthCheck.CheckHealthAsync(context, cancellationToken);
            });
        }
    }
}