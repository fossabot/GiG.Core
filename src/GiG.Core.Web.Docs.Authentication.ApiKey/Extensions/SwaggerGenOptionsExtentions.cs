using System;
using System.Collections.Generic;
using System.Linq;
using GiG.Core.Authentication.ApiKey.Abstractions;
using GiG.Core.Web.Authentication.ApiKey.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GiG.Core.Web.Docs.Authentication.ApiKey.Extensions
{
    internal static class SwaggerGenOptionsExtensions
    {
        internal static void AddApiKeySecurityDefinition(this SwaggerGenOptions options,
            ApiKeyOptions apiKeyAuthenticationOptions)
        {
            options.AddSecurityDefinition(SecuritySchemes.ApiKey, new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = Headers.ApiKey,
                Type = SecuritySchemeType.ApiKey,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = SecuritySchemes.ApiKey
                        }
                    },
                    new string[] { }
                }
            });
        }

        internal static void IncludeApiKeyAuthentication(this SwaggerGenOptions options, ApiKeyOptions apiKeyAuthenticationOptions)
        {
            options.AddApiKeySecurityDefinition(apiKeyAuthenticationOptions);
        }
    }
}