﻿using GiG.Core.Web.Docs.Abstractions;
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
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures Api Docs.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="ApiDocsOptions"/>.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="SwaggerGenOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfigurationSection configurationSection, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            services.Configure<ApiDocsOptions>(configurationSection);

            var apiDocsOptions = configurationSection?.Get<ApiDocsOptions>() ?? new ApiDocsOptions();
            if (!apiDocsOptions.IsEnabled)
            {
                return services;
            }
            
            return services
                .AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
                .AddSwaggerGen(c =>
                {
                    c.IncludeXmlComments(apiDocsOptions.IsXmlDocumentationEnabled);
                    c.IncludeFullNameCustomSchemaId();
                    c.IncludeForwardedForFilter(apiDocsOptions.IsForwardedForEnabled);
                    c.IncludeTenantIdFilter(apiDocsOptions.IsTenantIdEnabled);
                    c.OperationFilter<DeprecatedOperationFilter>();
                    configureOptions?.Invoke(c);
                });
        }

        /// <summary>
        /// Configures Api Docs.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="ApiDocsOptions"/>.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="ApiDocsOptions" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiDocs([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration, Action<SwaggerGenOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.ConfigureApiDocs(configuration.GetSection(ApiDocsOptions.DefaultSectionName), configureOptions);
        }
    }
}