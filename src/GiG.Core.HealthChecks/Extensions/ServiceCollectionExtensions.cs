using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureHealthChecks([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureHealthChecks(configuration.GetSection(HealthChecksOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureHealthChecks([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection?.Exists() != true) throw new ArgumentNullException(nameof(configurationSection));

            return services.Configure<HealthChecksOptions>(configurationSection);
        }

        /// <summary>
        /// Adds the <see cref="HealthCheckService" /> to the container, using the provided delegate to register
        /// health checks. This also includes the cached health checks functionality.
        /// </summary>
        /// <remarks>
        /// This operation is idempotent; multiple invocations will still only result in a single <see cref="HealthCheckService" /> instance in the <see cref="IServiceCollection" />. 
        /// It can be invoked multiple times in order to get access to the <see cref="IHealthChecksBuilder" /> in multiple places.
        /// </remarks>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <returns>The <see cref="IHealthChecksBuilder" />.</returns>
        public static IHealthChecksBuilder AddCachedHealthChecks([NotNull] this IServiceCollection services)
        {
            return services
                .AddMemoryCache()
                .AddHealthChecks();
        }
    }
}