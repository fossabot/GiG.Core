using GiG.Core.Authentication.ApiKey.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GiG.Core.Web.Docs.Authentication.ApiKey.Extensions
{
    internal static class SwaggerGenOptionsExtensions
    {
        internal static void IncludeApiKeyAuthentication(this SwaggerGenOptions options)
        {
            options.AddApiKeySecurityDefinition();
        }
        
        private static void AddApiKeySecurityDefinition(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(Constants.SecurityScheme, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = Constants.ApiKeyHeader,
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
                            Id = Constants.SecurityScheme
                        }
                    },
                    new string[] { }
                }
            });
        }
    }
}