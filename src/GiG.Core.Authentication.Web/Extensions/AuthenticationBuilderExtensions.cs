using GiG.Core.Authentication.Web.Abstractions;
using IdentityServer4.AccessTokenValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using System;

namespace GiG.Core.Authentication.Web.Extensions
{
    /// <summary>
    /// Authentication Builder Extensions.
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// Configures Identity Server Authentication using <see cref="ApiAuthenticationOptions" />.
        /// </summary>
        /// <param name="builder">The <see cref="AuthenticationBuilder" />.</param>
        /// <param name="options">The <see cref="ApiAuthenticationOptions" />.</param>
        /// <returns>The <see cref="AuthenticationBuilder" />.</returns>
        public static AuthenticationBuilder ConfigureApiAuthentication([NotNull] this AuthenticationBuilder builder, [NotNull] ApiAuthenticationOptions options)
        {
            if (options?.IsEnabled == true)
            {
                builder
                    .AddIdentityServerAuthentication(x =>
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

            return builder;
        }
    }
}