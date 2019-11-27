using GiG.Core.Providers.DateTime.Extensions;
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
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services for the Token Manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddTokenManager([NotNull] this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddTokenManager(configuration.GetSection(TokenManagerOptions.DefaultSectionName));
        }

        /// <summary>
        /// Adds the required services for the Token Manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
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
    }
}