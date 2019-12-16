using GiG.Core.Authentication.Abstractions;
using GiG.Core.Authentication.Web.Abstractions;
using GiG.Core.Web.Docs.Abstractions;
using GiG.Core.Web.Docs.Filters;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GiG.Core.Web.Docs.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="ApiDocsOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" />.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="SwaggerGenOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services,
            [NotNull] IConfigurationSection configurationSection, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            var apiDocsOptions = configurationSection?.Get<ApiDocsOptions>() ?? new ApiDocsOptions();
            services.Configure<ApiDocsOptions>(options =>
                {
                    options.Description = apiDocsOptions.Description;
                    options.IsEnabled = apiDocsOptions.IsEnabled;
                    options.IsForwardedForEnabled = apiDocsOptions.IsForwardedForEnabled;
                    options.Title = apiDocsOptions.Title;
                    options.Url = apiDocsOptions.Url;
                });

            if (!apiDocsOptions.IsEnabled)
            {
                return services;
            }

            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                //The format of the version added to the route URL: "'v'major"
                options.GroupNameFormat = "'v'V";

                //Tells swagger to replace the version in the controller route  
                options.SubstituteApiVersionInUrl = true;
            });

            return services
                .Configure<ApiDocsOptions>(configurationSection)
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(c =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    c.IncludeXmlComments();
                    c.IncludeFullNameCustomSchemaId();
                    c.IncludeForwardedForFilter(apiDocsOptions.IsForwardedForEnabled);
                    c.OperationFilter<DeprecatedOperationFilter>();
                    
                    c.IncludeAuthentication(serviceProvider);

                    configureOptions?.Invoke(c);
                });
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="ApiDocsOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" />.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="SwaggerGenOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services,
            [NotNull] IConfiguration configuration, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName), configureOptions);
        }

        private static void IncludeAuthentication(this SwaggerGenOptions options, ServiceProvider serviceProvider)
        {
            var apiAuthenticationConfig = serviceProvider.GetService<IOptions<ApiAuthenticationOptions>>();
            options.AddOAuth2SecurityDefinition(apiAuthenticationConfig?.Value);
        }

        private static void IncludeXmlComments(this SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            if (string.IsNullOrEmpty(xmlPath))
            {
                throw new ApplicationException(
                    "The following property is missing from your project; <GenerateDocumentationFile>true</GenerateDocumentationFile>.");
            }

            options.IncludeXmlComments(xmlPath);
        }

        private static void IncludeFullNameCustomSchemaId(this SwaggerGenOptions options)
        {
            options.CustomSchemaIds(x => x.FullName);
        }

        private static void IncludeForwardedForFilter(this SwaggerGenOptions options, bool isEnabled)
        {
            if (isEnabled)
            {
                options.OperationFilter<ForwardedForOperationFilter>();
            }
        }

        private static void AddOAuth2SecurityDefinition(this SwaggerGenOptions options, ApiAuthenticationOptions apiAuthenticationOptions)
        {
            if (apiAuthenticationOptions?.IsEnabled == true)
            {
                var scopes = new Dictionary<string, string>
                {
                    {apiAuthenticationOptions.ApiName, "This is the main scope needed to access API"}
                };

                if (apiAuthenticationOptions.Scopes?.Any() == true)
                {
                    foreach (var scope in apiAuthenticationOptions.Scopes.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (!scopes.ContainsKey(scope))
                        {
                            scopes.Add(scope, "This is a dependency scope needed to access API");
                        }
                    }
                }

                options.AddSecurityDefinition(SecuritySechemes.Oauth2, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(string.Concat(apiAuthenticationOptions.Authority, "/connect/authorize"), UriKind.Absolute),
                            Scopes = scopes,
                            TokenUrl = new Uri(string.Concat(apiAuthenticationOptions.Authority, "/connect/token"), UriKind.Absolute),
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
                                Id = SecuritySechemes.Oauth2 
                            }
                        },
                        scopes.Select(x => x.Key).ToList()
                    }
                });
            }

        }
    }
}