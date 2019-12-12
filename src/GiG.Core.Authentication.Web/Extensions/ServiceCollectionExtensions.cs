using GiG.Core.Authentication.Abstractions;
using GiG.Core.Authentication.Web.Abstractions;
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
        /// Registers a configuration instance which <see cref="ApiAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var apiAuthentiicationOptions = configurationSection?.Get<ApiAuthenticationOptions>() ?? new ApiAuthenticationOptions();
            services.Configure<ApiAuthenticationOptions>(configureOptions =>
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

            return services.ConfigureApiAuthentication(apiAuthentiicationOptions);
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="ApiAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiAuthentication([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureApiAuthentication(configuration.GetSection(ApiAuthenticationOptions.DefaultSectionName));
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="ApiAuthenticationOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="options">The <see cref="ApiAuthenticationOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiAuthentication([NotNull] this IServiceCollection services, [NotNull] ApiAuthenticationOptions options)
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
                    .ConfigureApiAuthentication(options);
            }

            return services;
        }
    }
}