﻿using GiG.Core.Providers.DateTime.Extensions;
using GiG.Core.TokenManager.Abstractions.Interfaces;
using GiG.Core.TokenManager.Abstractions.Models;
using GiG.Core.TokenManager.Implementation;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;

namespace GiG.Core.TokenManager.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Token Manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="TokenManagerOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddTokenManager([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddTokenManager(configuration.GetSection(TokenManagerOptions.DefaultSectionName));
        }

        /// <summary>
        /// Adds the Token Manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="TokenManagerOptions"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ConfigurationErrorsException"></exception>
        public static IServiceCollection AddTokenManager([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration Section '{configurationSection?.Path}' is incorrect.");

            services.AddUtcDateTimeProvider();
            services.AddHttpClient();

            services.Configure<TokenManagerOptions>(configurationSection);
            services.TryAddSingleton<ITokenClientFactory, TokenClientFactory>();
            services.TryAddSingleton<ITokenManagerFactory, TokenManagerFactory>();

            return services;
        }

        /// <summary>
        /// Adds the Token Manager Factory.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddTokenManagerFactory([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
         
            services.AddUtcDateTimeProvider();
            services.AddHttpClient();

            services.TryAddSingleton<ITokenClientFactory, TokenClientFactory>();
            services.TryAddSingleton<ITokenManagerFactory, TokenManagerFactory>();

            return services;
        }
    }
}