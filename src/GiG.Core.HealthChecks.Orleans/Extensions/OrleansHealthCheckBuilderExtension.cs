using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.HealthChecks.Orleans.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orleans;
using System;
using System.Collections.Generic;

namespace GiG.Core.HealthChecks.Orleans.Extensions
{
    /// <summary>
    /// HealthChecks Builder Extensions
    /// </summary>
    public static class OrleansHealthCheckBuilderExtension
    {
        private const string HealthCheckName = "OrleansHealthCheck";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="healthChecksBuilder">The <see cref="IHealthChecksBuilder" />.</param>
        /// <returns></returns>
        public static IHealthChecksBuilder AddOrleansHealthCheck([NotNull] this IHealthChecksBuilder healthChecksBuilder)
        {
            if (healthChecksBuilder == null) throw new ArgumentNullException(nameof(healthChecksBuilder));

            return healthChecksBuilder.Add(
                new HealthCheckRegistration(HealthCheckName, sp => new OrleansHealthCheck(sp.GetRequiredService<IClusterClient>()), 
                HealthStatus.Unhealthy, new List<string> { Constants.ReadyTag }));
        }
    }
}
