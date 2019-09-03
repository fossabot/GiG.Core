using GiG.Core.Abstractions.HealthCheck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;

namespace GiG.Core.Extensions.HealthCheck
{
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        ///  Adds a new cached health check with the specified name and implementation.
        ///  Adds the ready tag to the health check automatically.
        /// </summary>
        /// <typeparam name="T">The health check implementation type.</typeparam>
        /// <param name="healthChecksBuilder">The Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder.</param>
        /// <param name="name">The name of the health check.</param>
        /// <param name="failureStatus">The Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus that should be
        ///     reported when the health check reports a failure. If the provided value is null,
        ///     then Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy will
        ///     be reported.
        ///</param>
        /// <param name="tags">A list of tags that can be used to filter health checks.</param>
        /// <returns>The Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder.</returns>
        public static IHealthChecksBuilder AddCachedCheck<T>(this IHealthChecksBuilder healthChecksBuilder, string name, HealthStatus? failureStatus = null,
          IList<string> tags = null) where T : CachedHealthCheck
        {
            tags = tags ?? new List<string>();
            if (!tags.Contains(Constants.ReadyTag))
            {
                tags.Add(Constants.ReadyTag);
            }

            return healthChecksBuilder.AddCheck<T>(name, failureStatus, tags);
        }
    }
}
