﻿using GiG.Core.Hosting;
using GiG.Core.Web.Docs.Abstractions;
using GiG.Core.Web.Docs.Filters;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
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
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.Configure<ApiDocsOptions>(configurationSection);

            var docOptions = configurationSection.Get<ApiDocsOptions>() ?? new ApiDocsOptions();
            if (!docOptions.IsEnabled)
            {
                return services;
            }

            services.AddVersionedApiExplorer(options =>
            {
                //The format of the version added to the route URL: "'v'major"
                options.GroupNameFormat = "'v'V";

                //Tells swagger to replace the version in the controller route  
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            return services
                .Configure<ApiDocsOptions>(configurationSection)
                .AddSwaggerGen(c =>
                {
                    // Resolve the temporary IApiVersionDescriptionProvider service  
                    var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                    c.IncludeXmlComments();
                    c.IncludeFullNameCustomSchemaId();
                    c.IncludeForwardedForFilter(docOptions.IsForwardedForEnabled);
                    c.OperationFilter<DeprecatedOperationFilter>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        c.SwaggerDoc(description.GroupName, new OpenApiInfo
                        {
                            Title = docOptions.Title ?? ApplicationMetadata.Name,
                            Description = $"{docOptions.Description} {(description.IsDeprecated ? " - DEPRECATED." : "")}",
                            Version = ApplicationMetadata.Version
                        });
                    }

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
            [NotNull] IConfiguration configuration, Action<SwaggerGenOptions> configureOptions = null) =>
            services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName), configureOptions);

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
    }
}