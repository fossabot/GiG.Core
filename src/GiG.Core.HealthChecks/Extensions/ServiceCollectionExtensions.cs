using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="T:GiG.Core.HealthChecks.Abstractions.HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configuration">The configuration <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureHealthChecks([NotNull] this IServiceCollection services,
            [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureHealthChecks(configuration.GetSection(HealthChecksOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="T:GiG.Core.HealthChecks.Abstractions.HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configurationSection">The configuration section <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureHealthChecks([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.Configure<HealthChecksOptions>(configurationSection);
        }

        /// <summary>
        /// Adds the <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService" /> to the container, using the provided delegate to register
        /// health checks. This also includes the cached health checks functionality.
        /// </summary>
        /// <remarks>
        /// This operation is idempotent - multiple invocations will still only result in a single
        /// <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService" /> instance in the <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />. It can be invoked
        /// multiple times in order to get access to the <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" /> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the <see cref="T:Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService" /> to.</param>
        /// <returns>An instance of <see cref="T:Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder" /> from which health checks can be registered.</returns>
        public static IHealthChecksBuilder AddCachedHealthChecks([NotNull] this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddHealthChecks();
        }
    }
}