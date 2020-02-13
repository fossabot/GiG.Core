using GiG.Core.Web.Docs.Abstractions;
using GiG.Core.Web.Docs.Filters;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

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
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            var apiDocsOptions = configurationSection?.Get<ApiDocsOptions>() ?? new ApiDocsOptions();
            services.Configure<ApiDocsOptions>(options =>
            {
                options.Description = apiDocsOptions.Description;
                options.IsEnabled = apiDocsOptions.IsEnabled;
                options.IsForwardedForEnabled = apiDocsOptions.IsForwardedForEnabled;
                options.Title = apiDocsOptions.Title;
                options.Url = apiDocsOptions.Url;
                options.XTenantIdEnabled = apiDocsOptions.XTenantIdEnabled;
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
                .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(c =>
                {
                    c.IncludeXmlComments();
                    c.IncludeFullNameCustomSchemaId();
                    c.IncludeForwardedForFilter(apiDocsOptions.IsForwardedForEnabled);
                    c.IncludeXTenantIdFilter(apiDocsOptions.XTenantIdEnabled);
                    c.OperationFilter<DeprecatedOperationFilter>();
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
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName), configureOptions);
        }
    }
}