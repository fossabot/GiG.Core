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
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var apiAuthenticationOptions = configurationSection.Get<OAuthAuthenticationOptions>();

            if (apiAuthenticationOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' does not exist.");
            }

            if (!apiAuthenticationOptions.IsEnabled)
            {
                return services;
            }

            services.Configure<OAuthAuthenticationOptions>(configureOptions =>
            {
                configureOptions.IsEnabled = apiAuthenticationOptions.IsEnabled;
                configureOptions.Authority = apiAuthenticationOptions.Authority;
                configureOptions.ApiName = apiAuthenticationOptions.ApiName;
                configureOptions.ApiSecret = apiAuthenticationOptions.ApiSecret;
                configureOptions.Scopes = apiAuthenticationOptions.Scopes;
                configureOptions.SupportedTokens = apiAuthenticationOptions.SupportedTokens;
                configureOptions.RequireHttpsMetadata = apiAuthenticationOptions.RequireHttpsMetadata;
                configureOptions.LegacyAudienceValidation = apiAuthenticationOptions.LegacyAudienceValidation;
            });

            return services.AddOAuthAuthentication(apiAuthenticationOptions);
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureOAuthAuthentication(configuration.GetSection(OAuthAuthenticationOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="OAuthAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">The <see cref="OAuthAuthenticationOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddOAuthAuthentication([NotNull] this IServiceCollection services, [NotNull] OAuthAuthenticationOptions options)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.IsEnabled == true)
            {
                const string authenticationScheme = AuthenticationSchemes.Bearer;

                services
                    .AddAuthentication(x =>
                    {
                        x.DefaultScheme = authenticationScheme;
                        x.DefaultChallengeScheme = authenticationScheme;
                    })
                    .AddOAuthAuthentication(options);
            }

            return services;
        }
    }
}