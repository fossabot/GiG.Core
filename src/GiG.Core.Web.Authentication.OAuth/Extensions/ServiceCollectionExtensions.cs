using GiG.Core.Authentication.OAuth.Abstractions;
using GiG.Core.Web.Authentication.OAuth.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace GiG.Core.Web.Authentication.OAuth.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            var apiAuthenticationOptions = configurationSection.Get<OAuthAuthenticationOptions>();
            if (apiAuthenticationOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' does not exist.");
            }

            services.Configure<OAuthAuthenticationOptions>(configurationSection);

            return services.AddOAuthAuthentication(apiAuthenticationOptions, schemeName);
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, string schemeName = Constants.SecurityScheme)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            return services.ConfigureOAuthAuthentication(configuration.GetSection(OAuthAuthenticationOptions.DefaultSectionName), schemeName);
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">The <see cref="OAuthAuthenticationOptions" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] OAuthAuthenticationOptions options, string schemeName)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            services
                .AddAuthentication(x =>
                {
                    x.DefaultScheme = schemeName;
                    x.DefaultChallengeScheme = schemeName;
                })
                .AddOAuthAuthentication(options, schemeName);

            return services;
        }
    }
}