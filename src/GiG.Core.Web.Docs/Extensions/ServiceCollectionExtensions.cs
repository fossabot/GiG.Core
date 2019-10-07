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
        /// Registers a configuration instance which <see cref="T:GiG.Core.HealthChecks.Abstractions.HealthChecksOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configurationSection">The configuration section <see cref="T:Microsoft.Extensions.Configuration.IConfigurationSection" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection)
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
                });
        }

        /// <summary>
        /// Registers a configuration instance which <see cref="T:GiG.Core.Web.Docs.Abstractions.ApiDocsOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configuration">The configuration <see cref="T:Microsoft.Extensions.Configuration.IConfiguration" />.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
            => services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName));

        private static void IncludeXmlComments(this SwaggerGenOptions options)
        {
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

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