using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Health Checks Builder Extensions.
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
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
        public static IHealthChecksBuilder AddReadyCheck<T>([NotNull] this IHealthChecksBuilder builder, [NotNull] string name, 
            HealthStatus? healthStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));

            return builder.AddCheck<T>(name, healthStatus, new [] { Constants.ReadyTag });
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
        public static IHealthChecksBuilder AddReadyCheck([NotNull] this IHealthChecksBuilder builder, [NotNull] string name,
            [NotNull] IHealthCheck instance, HealthStatus? healthStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, healthStatus, new [] { Constants.ReadyTag });
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
        public static IHealthChecksBuilder AddLiveCheck<T>([NotNull] this IHealthChecksBuilder builder, [NotNull] string name, 
            HealthStatus? healthStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            
            return builder.AddCheck<T>(name, healthStatus, new [] { Constants.LiveTag });
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
        public static IHealthChecksBuilder AddLiveCheck([NotNull] this IHealthChecksBuilder builder, [NotNull] string name,
            [NotNull] IHealthCheck instance, HealthStatus? healthStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, healthStatus, new [] { Constants.LiveTag });
        }
    }
}