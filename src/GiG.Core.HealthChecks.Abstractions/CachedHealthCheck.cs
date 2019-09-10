using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GiG.Core.HealthChecks.Abstractions
{
    /// <summary>
    /// Base Abstract Class for Cached Health Checks.
    /// </summary>
    public abstract class CachedHealthCheck : IHealthCheck
    {
        private readonly IMemoryCache _memoryCache;
        private readonly long _cacheExpirationMs;
        private readonly string _cacheKey;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in MS.</param>
        protected CachedHealthCheck(IMemoryCache memoryCache, long cacheExpirationMs = 1000)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMs = cacheExpirationMs;
            _cacheKey = GetType().FullName;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="memoryCache">The Memory Cache Provider.</param>
        /// <param name="name">The name of the instance of the Health Check.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in MS.</param>
        protected CachedHealthCheck(IMemoryCache memoryCache, string name, long cacheExpirationMs = 1000)
        {
            _memoryCache = memoryCache;
            _cacheExpirationMs = cacheExpirationMs;
            _cacheKey = name;
        }

        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(_cacheKey, async result =>
            {
                result.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMilliseconds(_cacheExpirationMs);

                return await DoHealthCheckAsync(context, cancellationToken);
            });
        }

        /// <summary>
        /// Runs the health check, returning the status of the component being checked.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> that can be used to cancel the health check.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task`1" /> that completes when the health check has finished, yielding the status of the component being checked.</returns>
        protected abstract Task<HealthCheckResult> DoHealthCheckAsync(HealthCheckContext context,
            CancellationToken cancellationToken = default);
    }
}