using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Base Abstract Class for Cached Health Checks.
    /// </summary>
    public abstract class CachedHealthCheck : IHealthCheck
    {
        private const long DefaultCacheExpirationMilliseconds = 500;

        private readonly IMemoryCache _memoryCache;
        private readonly long _cacheExpirationMs;
        private readonly string _cacheKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="cacheExpirationMilliseconds">The Expiration of the Cache in milliseconds.</param>
        protected CachedHealthCheck(IMemoryCache memoryCache, long cacheExpirationMilliseconds = DefaultCacheExpirationMilliseconds)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMs = cacheExpirationMilliseconds;
            _cacheKey = GetType().FullName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="name">The HealthCheck name.</param>
        /// <param name="cacheExpirationMilliseconds">The Expiration of the Cache in milliseconds.</param>
        protected CachedHealthCheck(IMemoryCache memoryCache, string name, long cacheExpirationMilliseconds = DefaultCacheExpirationMilliseconds)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMs = cacheExpirationMilliseconds;
            _cacheKey = name;
        }

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(_cacheKey, async result =>
            {
                result.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(_cacheExpirationMs);

                return await DoHealthCheckAsync(context, cancellationToken);
            });
        }

        /// <summary>
        /// Runs the HealthCheck, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A <see cref="HealthCheckContext" />.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" />.</param>
        /// <returns>A <see cref="Task" /> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        protected abstract Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context, CancellationToken cancellationToken = default);
    }
}