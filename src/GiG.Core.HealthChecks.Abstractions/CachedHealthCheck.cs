using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Base Abstract Class for Cached Health Checks
    /// </summary>
    public abstract class CachedHealthCheck : IHealthCheck
    {
        private readonly IMemoryCache _memoryCache;
        private readonly long _cacheExpirationMS;
        private readonly string _cacheKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="cacheExpirartionMs">The Expiration of the Cache in MS.</param>
        protected CachedHealthCheck(IMemoryCache memoryCache, long cacheExpirartionMs = 1000)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMS = cacheExpirartionMs;
            _cacheKey = GetType().FullName;
        }

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(_cacheKey, async result =>
            {
                result.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(_cacheExpirationMS);
                var res = await DoHealthCheckAsync(context, cancellationToken);
                return res;
            });
        }

        /// <summary>
        /// Actual Health Check implementation performed when the Health Check Result is not found in Cache.
        /// </summary>
        /// <param name="context"> A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A CancellationToken that can be used to cancel the health check.</param>
        /// <returns>The Health Check result</returns>
        protected abstract Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken = default);
    }
}