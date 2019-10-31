using GiG.Core.Web.Security.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Web.Security.Hmac.MultiTenant.Extensions
{
    public static class ServiceCollectionExtensions
    {
       
        /// <summary>
         /// Adds multitenant option provider for <see cref="HmacAuthenticationHandler" /> functionality.
         /// </summary>
         /// <param name="services">The <see cref="IServiceCollection" />.</param>        
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
