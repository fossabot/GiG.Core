using GiG.Core.Security.Cryptography;
using GiG.Core.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Abstractions;
using GiG.Core.Web.Authentication.Hmac.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;
using System.Linq;

// ReSharper disable ObjectCreationAsStatement

namespace GiG.Core.Web.Authentication.Hmac.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="HmacAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddHmacAuthentication([NotNull] this IServiceCollection services, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services
                .AddAuthentication(schemeName)
                .AddScheme<HmacRequirement, HmacAuthenticationHandler>(schemeName, x => new HmacOptions());

            services.TryAddSingleton<IHashProvider, SHA256HashProvider>();
            services.TryAddSingleton<IHmacSignatureProvider, HmacSignatureProvider>();
            services.TryAddSingleton<IHashProviderFactory, HashProviderFactory>();
            services.TryAddSingleton<Func<string, IHashProvider>>(x =>
                hash => x.GetServices<IHashProvider>().FirstOrDefault(sp => sp.Name.Equals(hash)));

            return services;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />Configuration section for hmac settings.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureDefaultHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services.TryAddScoped<IHmacOptionsProvider, DefaultOptionsProvider>();
            services.Configure<HmacOptions>(schemeName, configurationSection);

            return services;
        }

        /// <summary>
        /// Adds option provider for <see cref="HmacAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" />Configuration for hmac settings.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureDefaultHmacOptionProvider([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services.ConfigureDefaultHmacOptionProvider(configuration.GetSection(HmacOptions.DefaultSectionName), schemeName);

            return services;
        }
    }
}