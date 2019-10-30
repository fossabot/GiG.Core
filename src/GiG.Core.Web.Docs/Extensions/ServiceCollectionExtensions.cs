using GiG.Core.Hosting;
using GiG.Core.Web.Docs.Abstractions;
using GiG.Core.Web.Docs.Filters;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
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

            return services
                .Configure<ApiDocsOptions>(configurationSection)
                .AddSwaggerGen(c =>
                {
                    c.IncludeXmlComments();
                    c.IncludeFullNameCustomSchemaId();
                    c.IncludeForwardedForFilter(docOptions.IsForwardedForEnabled);
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = docOptions.Title ?? ApplicationMetadata.Name,
                        Description = docOptions.Description,
                        Version = ApplicationMetadata.Version
                    });
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
            => services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName), configureOptions);

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