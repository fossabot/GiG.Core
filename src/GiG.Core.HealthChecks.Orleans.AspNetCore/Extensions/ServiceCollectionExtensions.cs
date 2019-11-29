using GiG.Core.HealthChecks.Orleans.Abstractions;
using GiG.Core.HealthChecks.Orleans.AspNetCore.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Orleans Health Check Hosted Service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddHealthChecksService([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.ConfigureOrleansHealthChecks(configuration);
            services.AddHostedService<HealthCheckService>();

            return services;
        }

        /// <summary>
        /// Adds the Orleans Health Check Hosted Service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddHealthChecksService([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.ConfigureOrleansHealthChecks(configurationSection);
            services.AddHostedService<HealthCheckService>();

            return services;
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        internal static IServiceCollection ConfigureOrleansHealthChecks([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureOrleansHealthChecks(configuration.GetSection(HealthChecksOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        internal static IServiceCollection ConfigureOrleansHealthChecks([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var healthCheckOptions = configurationSection?.Get<HealthChecksOptions>() ?? new HealthChecksOptions();

            return services.Configure<HealthChecksOptions>(h =>
            {
                h.DomainFilter = healthCheckOptions.DomainFilter;
                h.Port = healthCheckOptions.Port;
                h.HostSelf = healthCheckOptions.HostSelf;
            });
        }
    }
}