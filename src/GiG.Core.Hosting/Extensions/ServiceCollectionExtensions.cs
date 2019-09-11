using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Service Collection Extensions for Hosting.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds registration of Metadata Accessor for Application.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>Returns the <see cref="IServiceCollection"/> so that more methods can be chained.</returns>
        public static IServiceCollection AddApplicationMetadataAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IApplicationMetadataAccessor, ApplicationMetadataAccessor>();

            return services;
        }

        /// <summary>
        /// Binds the Info Management Options to the passed configuration.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configuration">The configuration <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureInfoManagement([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureInfoManagement(configuration.GetSection(InfoManagementOptions.DefaultSectionName));

        }

        /// <summary>
        /// Binds the Info Management Options to the passed configuration.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configurationSection">The configuration section <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureInfoManagement([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            return services.Configure<InfoManagementOptions>(configurationSection);
        }
    }
}