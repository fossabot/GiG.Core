using GiG.Core.MultiTenant.Activity.Extensions;
using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.MultiTenant.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace GiG.Core.Web.Authentication.Hmac.MultiTenant.Extensions
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
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> for Hmac MultiTenant configuration.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureMultiTenantHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services.AddActivityTenantAccessor();
            services.TryAddScoped<IHmacOptionsProvider, MultiTenantOptionProvider>();
            services.Configure<Dictionary<string,HmacOptions>>(schemeName, configurationSection);

            return services;
        }

        /// <summary>
        /// Adds multi-tenant option provider for HmacAuthenticationHandler functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureMultiTenantHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services.ConfigureMultiTenantHmacOptionProvider(configuration.GetSection(HmacOptions.DefaultSectionName), schemeName);

            return services;
        }
    }
}
