using GiG.Core.Authentication.OAuth.Abstractions;
using GiG.Core.Web.Authentication.OAuth.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GiG.Core.Web.Docs.Authentication.OAuth.Extensions
{
    internal static class SwaggerGenOptionsExtensions
    {
        internal static void IncludeOAuthAuthentication(this SwaggerGenOptions options,
            OAuthAuthenticationOptions oAuthAuthenticationOptions)
        {
            options.AddOAuth2SecurityDefinition(oAuthAuthenticationOptions);
        }

        private static void AddOAuth2SecurityDefinition(this SwaggerGenOptions options,
            OAuthAuthenticationOptions oauthAuthenticationOptions)
        {
            var scopes = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(oauthAuthenticationOptions.ApiName))
            {
                scopes.Add(oauthAuthenticationOptions.ApiName, "This is the main scope needed to access API");
            }

            if (oauthAuthenticationOptions.Scopes?.Any() == true)
            {
                foreach (var scope in oauthAuthenticationOptions.Scopes.Split(new[] {" "},
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!scopes.ContainsKey(scope))
                    {
                        scopes.Add(scope, "This is a dependency scope needed to access API");
                    }
                }
            }

            options.AddSecurityDefinition(Constants.SecurityScheme, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl =
                            new Uri(string.Concat(oauthAuthenticationOptions.Authority, "/connect/authorize"),
                                UriKind.Absolute),
                        Scopes = scopes,
                        TokenUrl = new Uri(string.Concat(oauthAuthenticationOptions.Authority, "/connect/token"),
                            UriKind.Absolute)
                    }
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = Constants.SecurityScheme
                        }
                    },
                    scopes.Select(x => x.Key).ToList()
                }
            });
        }
    }
}