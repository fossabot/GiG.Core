using GiG.Core.Security.Cryptography;
using GiG.Core.Web.Security.Hmac.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;

namespace GiG.Core.Web.Security.Hmac.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="HmacAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddHmacAuthentication([NotNull]this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddAuthentication("hmac").AddScheme<HmacRequirement, HmacAuthenticationHandler>("hmac",x=> new HmacOptions());
            
            services
                .TryAddTransient<IHashProvider, SHA256HashProvider>();
            services.TryAddTransient<IHashProviderFactory, HashProviderFactory>();
            services.TryAddSingleton<Func<string, IHashProvider>>(x =>
                (hash) => x.GetServices<IHashProvider>().FirstOrDefault(sp => sp.Name.Equals(hash))
                );

            return services;
        }
        /// <summary>
         /// Adds option provider for <see cref="HmacAuthenticationHandler" /> functionality.
         /// </summary>
         /// <param name="services">The <see cref="IServiceCollection" />.</param>        
         /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureDefaultHmacOptionProvider([NotNull]this IServiceCollection services, [NotNull]IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.TryAddTransient<IHmacOptionsProvider, DefaultTenantOptionsProvider>();
            services.Configure<HmacOptions>(configurationSection);

            return services;
        }
    }
}
