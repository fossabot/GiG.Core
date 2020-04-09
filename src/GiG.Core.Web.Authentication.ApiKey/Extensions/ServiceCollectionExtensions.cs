﻿using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;

namespace GiG.Core.Web.Authentication.ApiKey.Extensions
{
    /// <summary>
    /// <see cref="IServiceCollection"/> for <see cref="ApiKeyAuthenticationHandler"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddApiKeyAuthentication([NotNull]this IServiceCollection services)
        {
            // Guards
            if (services == null) throw new ArgumentNullException(nameof(services));

            services
                .AddAuthentication(ApiKeyAuthenticationOptions.DefaultScheme)
                .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.DefaultScheme, x => new ApiKeyAuthenticationOptions());
                
            services
                .TryAddSingleton<IAuthorizedApiKeysProvider, AuthorizedApiKeysProvider>();

            return services;
        }

        /// <summary>
        /// Adds option provider for <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />Configuration section for api key settings.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiKeyOptions([NotNull]this IServiceCollection services, [NotNull]IConfigurationSection configurationSection)
        {
            // Guards
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            // Configuration validation
            if (configurationSection.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' does not exist.");

            services.Configure<ApiKeyOptions>(configurationSection);

            return services;
        }
        /// <summary>
        /// Adds option provider for <see cref="ApiKeyAuthenticationHandler" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" />Configuration for api key settings.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiKeyOptions([NotNull]this IServiceCollection services, [NotNull]IConfiguration configuration)
        {
            // Guards
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            services.ConfigureApiKeyOptions(configuration.GetSection(ApiKeyOptions.DefaultSectionName));

            return services;
        }
    }
}
