using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Health Checks Builder Extensions.
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        ///  <summary>
        ///   Adds a new Cached HealthCheck with the specified name and implementation.
        ///  </summary>
        ///  <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        ///  <param name="name">The HealthCheck name.</param>
        /// <param name="factory">Returns the <see cref="IHealthCheck"/> instance.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check reports a failure. If the provided value
        /// is <c>null</c>, then <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter health checks.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in milliseconds.</param>
        ///  <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddCachedCheck([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, [NotNull] Func<IServiceProvider, IHealthCheck> factory, HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null, ulong cacheExpirationMs = CachedHealthCheck.DefaultCacheExpirationMs)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            return builder.Add(new HealthCheckRegistration(name, serviceProvider =>
            {
                var instance = factory(serviceProvider) ?? throw new ArgumentNullException(nameof(factory));
                
                var memoryCache = serviceProvider.GetService<IMemoryCache>();
                if (memoryCache == null) throw new ApplicationException("The following service is missing; Add services.AddCachedHealthChecks()");

                return new CachedHealthCheck(instance, memoryCache, cacheExpirationMs);
            }, failureStatus, tags));
        }
        
        ///  <summary>
        ///   Adds a new Cached HealthCheck with the specified name and implementation.
        ///  </summary>
        ///  <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        ///  <param name="name">The HealthCheck name.</param>
        /// <param name="instance">An <see cref="IHealthCheck"/> instance.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check reports a failure. If the provided value
        /// is <c>null</c>, then <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter health checks.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in milliseconds.</param>
        ///  <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddCachedCheck([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, IHealthCheck instance, HealthStatus? failureStatus = null,
            IEnumerable<string> tags = null, ulong cacheExpirationMs = CachedHealthCheck.DefaultCacheExpirationMs)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));

            return AddCachedCheck(builder, name, _ => instance, failureStatus, tags, cacheExpirationMs);
        }

        ///  <summary>
        ///   Adds a new Cached HealthCheck with the specified name and implementation.
        ///  </summary>
        ///  <typeparam name="T">The HealthCheck type.</typeparam>
        ///  <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        ///  <param name="name">The HealthCheck name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check reports a failure. If the provided value
        /// is <c>null</c>, then <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter health checks.</param>
        /// <param name="cacheExpirationMs">The Expiration of the Cache in milliseconds.</param>
        ///  <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddCachedCheck<T>([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, HealthStatus? failureStatus = null, IEnumerable<string> tags = null, 
            ulong cacheExpirationMs = CachedHealthCheck.DefaultCacheExpirationMs)
            where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            
            return builder.AddCachedCheck(name, ActivatorUtilities.GetServiceOrCreateInstance<T>, failureStatus, tags, cacheExpirationMs);
        }

        /// <summary>
        ///  Adds a new HealthCheck with the specified name and implementation.
        ///  Adds the Ready Tag to the HealthCheck automatically.
        /// </summary>
        /// <typeparam name="T">The HealthCheck type.</typeparam>
        /// <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        /// <param name="name">The HealthCheck name.</param>
        /// <param name="healthStatus">The <see cref="HealthStatus" /> that should be
        ///     reported when the HealthCheck reports a failure. If the provided value is null,
        ///     then <see cref="HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddReadyCheck<T>([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, HealthStatus? healthStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));

            return builder.AddCheck<T>(name, healthStatus, new[] {Constants.ReadyTag});
        }

        /// <summary>
        ///  Adds a new HealthCheck with the specified name and implementation.
        ///  Adds the Ready Tag to the HealthCheck automatically.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        /// <param name="name">The HealthCheck name.</param>
        /// <param name="instance">The HealthCheck instance.</param>
        /// <param name="healthStatus">The <see cref="HealthStatus" /> that should be
        ///     reported when the HealthCheck reports a failure. If the provided value is null,
        ///     then <see cref="HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddReadyCheck([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, [NotNull] IHealthCheck instance, HealthStatus? healthStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, healthStatus, new[] {Constants.ReadyTag});
        }

        /// <summary>
        ///  Adds a new HealthCheck with the specified name and implementation.
        ///  Adds the Live Tag to the HealthCheck automatically.
        /// </summary>
        /// <typeparam name="T">The HealthCheck type.</typeparam>
        /// <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        /// <param name="name">The HealthCheck name.</param>
        /// <param name="healthStatus">The <see cref="HealthStatus" /> that should be
        ///     reported when the HealthCheck reports a failure. If the provided value is null,
        ///     then <see cref="HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddLiveCheck<T>([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, HealthStatus? healthStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));

            return builder.AddCheck<T>(name, healthStatus, new[] {Constants.LiveTag});
        }

        /// <summary>
        ///  Adds a new HealthCheck with the specified name and implementation.
        ///  Adds the Live Tag to the HealthCheck automatically.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder" />.</param>
        /// <param name="name">The HealthCheck name.</param>
        /// <param name="instance">The HealthCheck instance.</param>
        /// <param name="healthStatus">The <see cref="HealthStatus" /> that should be
        ///     reported when the HealthCheck reports a failure. If the provided value is null,
        ///     then <see cref="HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddLiveCheck([NotNull] this IHealthChecksBuilder builder,
            [NotNull] string name, [NotNull] IHealthCheck instance, HealthStatus? healthStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, healthStatus, new[] {Constants.LiveTag});
        }
    }
}