using GiG.Core.Web.Security.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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
         /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureDefaultHmacOptionProvider([NotNull]this IServiceCollection services, [NotNull]IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.TryAddSingleton<IHmacOptionsProvider, MultiTenantOptionProvider>();
            services.Configure<HmacOptions>(configurationSection);

            return services;
        }
    }
}
