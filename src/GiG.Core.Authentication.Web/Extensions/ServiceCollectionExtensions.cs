using GiG.Core.Authentication.Abstractions;
using GiG.Core.Web.Authentication.OAuth.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Authentication.Web.Extensions
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

            var apiAuthentiicationOptions = configurationSection?.Get<OAuthAuthenticationOptions>() ?? new OAuthAuthenticationOptions();
            services.Configure<OAuthAuthenticationOptions>(configureOptions =>
            {
                configureOptions.IsEnabled = apiAuthentiicationOptions.IsEnabled;
                configureOptions.Authority = apiAuthentiicationOptions.Authority;
                configureOptions.ApiName = apiAuthentiicationOptions.ApiName;
                configureOptions.ApiSecret = apiAuthentiicationOptions.ApiSecret;
                configureOptions.Scopes = apiAuthentiicationOptions.Scopes;
                configureOptions.SupportedTokens = apiAuthentiicationOptions.SupportedTokens;
                configureOptions.RequireHttpsMetadata = apiAuthentiicationOptions.RequireHttpsMetadata;
                configureOptions.LegacyAudienceValidation = apiAuthentiicationOptions.LegacyAudienceValidation;
            });

            if (!apiAuthentiicationOptions.IsEnabled)
            {
                return services;
            }

            return services.AddOAuthAuthentication(apiAuthentiicationOptions);
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