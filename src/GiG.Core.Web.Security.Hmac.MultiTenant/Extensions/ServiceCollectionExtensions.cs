using GiG.Core.Web.Security.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using GiG.Core.Web.Security.Hmac.MultiTenant.Internal;

namespace GiG.Core.Web.Security.Hmac.MultiTenant.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"></see> extensions for HmacAuthenticationHandler.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds multi-tenant option provider for HmacAuthenticationHandler functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> for Hmac Multitenant configuration.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureMultiTenantHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull]IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.TryAddSingleton<IHmacOptionsProvider, MultiTenantOptionProvider>();
            services.Configure<Dictionary<string,HmacOptions>>(configurationSection);

            return services;
        }
        /// <summary>
        /// Adds multi-tenant option provider for HmacAuthenticationHandler functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureMultiTenantHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull]IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.ConfigureMultiTenantHmacOptionProvider(configuration.GetSection(HmacOptions.DefaultSectionName));

            return services;
        }
    }
}
