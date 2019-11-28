using GiG.Core.HealthChecks.Orleans.Internal;
using GiG.Core.Orleans.Silo.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Hosting;
using System;

namespace GiG.Core.HealthChecks.Orleans.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds the Orleans HealthCheck.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder" />.</param>
        /// <returns>The <see cref="ISiloBuilder" />.</returns>
        public static ISiloBuilder AddHealthCheckDependencies([NotNull] this ISiloBuilder siloBuilder)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));

            siloBuilder.ConfigureServices(services => services.AddHealthChecks().AddOrleansHealthCheck());
            return siloBuilder.AddAssemblies(typeof(HealthCheckGrain));
        }
    }
}