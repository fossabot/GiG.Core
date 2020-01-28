using GiG.Core.ApplicationMetrics.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.ApplicationMetrics.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="ApplicationMetricsOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApplicationMetrics([NotNull] this IServiceCollection services,
            [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureApplicationMetrics(configuration.GetSection(ApplicationMetricsOptions.DefaultSectionName));
        }
        
        /// <summary>
        /// Registers a configuration instance which <see cref="ApplicationMetricsOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApplicationMetrics([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var applicationMetricsOptions = configurationSection.Get<ApplicationMetricsOptions>() ?? new ApplicationMetricsOptions();

            return services.Configure<ApplicationMetricsOptions>(options =>
            {
                options.Url = applicationMetricsOptions.Url;
                options.IsEnabled = applicationMetricsOptions.IsEnabled;
            });
        }
    }
}