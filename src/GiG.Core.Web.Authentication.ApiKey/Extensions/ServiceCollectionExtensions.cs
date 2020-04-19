using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;

// ReSharper disable ObjectCreationAsStatement

namespace GiG.Core.Web.Authentication.ApiKey.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddApiKeyAuthentication([NotNull] this IServiceCollection services, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services
                .AddAuthentication(schemeName)
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(schemeName, x => new ApiKeyAuthenticationOptions());
                
            services
                .TryAddSingleton<IAuthorizedApiKeysProvider, AuthorizedApiKeysProvider>();

            return services;
        }

        /// <summary>
        /// Adds option provider for <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />Configuration section for api key settings.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiKeyOptions([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            if (configurationSection.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection.Path}' does not exist.");

            services.Configure<ApiKeyOptions>(schemeName, configurationSection);

            return services;
        }
        /// <summary>
        /// Adds option provider for <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" />Configuration for api key settings.</param>        
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiKeyOptions([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services.ConfigureApiKeyOptions(configuration.GetSection(ApiKeyOptions.DefaultSectionName), schemeName);

            return services;
        }
    }
}
