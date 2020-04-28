using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Hosting.Tests.Unit")]
namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Info Management.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="InfoManagementOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureInfoManagement([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureInfoManagement(configuration.GetSection(InfoManagementOptions.DefaultSectionName));
        }

        /// <summary>
        /// Configures Info Management.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="InfoManagementOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureInfoManagement([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            
            return services.Configure<InfoManagementOptions>(configurationSection);
        }

        /// <summary>
        /// Adds the Application Metadata Accessor.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        internal static IServiceCollection AddApplicationMetadataAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IApplicationMetadataAccessor, ApplicationMetadataAccessor>();

            return services;
        }
    }
}