using System;
using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Health Checks Builder Extensions
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        ///  Adds a new health check with the specified name and implementation.
        ///  Adds the ready tag to the health check automatically.
        /// </summary>
        /// <typeparam name="T">The health check implementation type.</typeparam>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="failureStatus">The <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus" /> that should be
        ///     reported when the health check reports a failure. If the provided value is null,
        ///     then <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddReadyCheck<T>([NotNull] this IHealthChecksBuilder builder, [NotNull] string name, 
            HealthStatus? failureStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (name == null) throw new ArgumentNullException(nameof(name));

            return builder.AddCheck<T>(name, failureStatus, new [] { Constants.ReadyTag });
        }

        /// <summary>
        ///  Adds a new health check with the specified name and implementation.
        ///  Adds the ready tag to the health check automatically.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="instance">The Health Check Instance</param>
        /// <param name="failureStatus">The <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus" /> that should be
        ///     reported when the health check reports a failure. If the provided value is null,
        ///     then <see cref="Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddReadyCheck([NotNull] this IHealthChecksBuilder builder, [NotNull] string name,
            IHealthCheck instance, HealthStatus? failureStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, failureStatus, new [] { Constants.ReadyTag });
        }
        
        /// <summary>
        ///  Adds a new health check with the specified name and implementation.
        ///  Adds the live tag to the health check automatically.
        /// </summary>
        /// <typeparam name="T">The health check implementation type.</typeparam>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="failureStatus">The <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus" /> that should be
        ///     reported when the health check reports a failure. If the provided value is null,
        ///     then <see cref="Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddLiveCheck<T>([NotNull] this IHealthChecksBuilder builder, [NotNull] string name, 
            HealthStatus? failureStatus = null) where T : class, IHealthCheck
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (name == null) throw new ArgumentNullException(nameof(name));
            
            return builder.AddCheck<T>(name, failureStatus, new [] { Constants.LiveTag });
        }

        /// <summary>
        ///  Adds a new health check with the specified name and implementation.
        ///  Adds the live tag to the health check automatically.
        /// </summary>
        /// <param name="builder">The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="instance">The Health Check Instance</param>
        /// <param name="failureStatus">The <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus" /> that should be
        ///     reported when the health check reports a failure. If the provided value is null,
        ///     then <see cref="Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy" /> will
        ///     be reported.
        ///</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddLiveCheck([NotNull] this IHealthChecksBuilder builder, [NotNull] string name,
            IHealthCheck instance, HealthStatus? failureStatus = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return builder.AddCheck(name, instance, failureStatus, new [] { Constants.LiveTag });
        }
    }
}
