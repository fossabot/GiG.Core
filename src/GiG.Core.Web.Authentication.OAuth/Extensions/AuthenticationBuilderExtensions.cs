using GiG.Core.Web.Authentication.OAuth.Abstractions;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System;

namespace GiG.Core.Web.Authentication.OAuth.Extensions
{
    /// <summary>
    /// Authentication Builder Extensions.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Configures Identity Server Authentication using <see cref="OAuthAuthenticationOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder" />.</param>
        /// <param name="options">The <see cref="OAuthAuthenticationOptions" />.</param>
        /// <param name="schemeName">The scheme name.</param>
        /// <returns>The <see cref="AuthenticationBuilder" />.</returns>
        public static AuthenticationBuilder AddOAuthAuthentication([NotNull] this AuthenticationBuilder builder, [NotNull] OAuthAuthenticationOptions options, string schemeName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(schemeName)) throw new ArgumentException($"'{nameof(schemeName)}' must not be null, empty or whitespace.", nameof(schemeName));

            return builder.AddIdentityServerAuthentication(schemeName, x =>
            {
                x.Authority = options.Authority;
                x.ApiName = options.ApiName;
                x.ApiSecret = options.ApiSecret;
                x.SupportedTokens =
                    Enum.TryParse(options.SupportedTokens, out SupportedTokens supportedToken)
                        ? supportedToken
                        : SupportedTokens.Both;
                x.RequireHttpsMetadata = options.RequireHttpsMetadata;
                x.LegacyAudienceValidation = options.LegacyAudienceValidation;
                x.JwtValidationClockSkew = TimeSpan.Zero;
            });
        }
    }
}