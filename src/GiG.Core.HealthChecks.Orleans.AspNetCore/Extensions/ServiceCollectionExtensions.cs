using GiG.Core.HealthChecks.Orleans.Abstractions;
using GiG.Core.HealthChecks.Orleans.AspNetCore.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.HealthChecks.Orleans.AspNetCore.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Orleans Health Check Hosted Service.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="HealthCheckOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddOrleansHealthChecksSelfHosted([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
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
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="HealthCheckOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddOrleansHealthChecksSelfHosted([NotNull] this IServiceCollection services, IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.ConfigureOrleansHealthChecks(configurationSection);
            services.AddHostedService<HealthCheckService>();

            return services;
        }

        /// <summary>
        /// Configures the Orleans Health Checks.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="HealthCheckOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        internal static IServiceCollection ConfigureOrleansHealthChecks([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureOrleansHealthChecks(configuration.GetSection(HealthCheckOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures the Orleans Health Checks.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="HealthCheckOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        internal static IServiceCollection ConfigureOrleansHealthChecks([NotNull] this IServiceCollection services, IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.Configure<HealthCheckOptions>(configurationSection);
        }
    }
}