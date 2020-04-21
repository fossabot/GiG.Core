using GiG.Core.Web.Docs.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Configuration;
using System.Text;

namespace GiG.Core.Web.Docs.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Documentation to API.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="SwaggerUIOptions" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseApiDocs([NotNull] this IApplicationBuilder builder,
            Action<SwaggerUIOptions> configureOptions = null)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = builder.ApplicationServices.GetService<IOptions<ApiDocsOptions>>()?.Value;
            if (options == null) throw new ConfigurationErrorsException("ConfigureApiDocs need to be registered");

            if (!options.IsEnabled)
            {
                return builder;
            }

            if (string.IsNullOrEmpty(options.Url))
            {
                throw new ConfigurationErrorsException($"{nameof(options.Url)} cannot be null or empty");
            }

            if (options.Url.StartsWith("/"))
            {
                throw new ConfigurationErrorsException($"{nameof(options.Url)} must not start with a slash");
            }

            // Temporary fix until Swagger bug is fixed - https://github.com/aspnet/AspNetCore/issues/10514
            var endpointPrefix = new StringBuilder();
            for (var i = 0; i < options.Url.Split('/').Length; i++)
            {
                endpointPrefix.Append("../");
            }

            return builder
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.AddSwaggerEndpoint(endpointPrefix.ToString(),
                        builder.ApplicationServices.GetService<IApiVersionDescriptionProvider>());
                    c.ShowExtensions();
                    c.RoutePrefix = options.Url;
                    c.DisplayRequestDuration();
                    configureOptions?.Invoke(c);
                });
        }

        private static void AddSwaggerEndpoint(this SwaggerUIOptions options, string prefix, IApiVersionDescriptionProvider provider = null)
        {
            if (provider == null)
            {
                options.AddSwaggerEndpoint(prefix, "v1");
                
                return;
            }

            foreach (var description in provider.ApiVersionDescriptions)
            {
                // build a swagger endpoint for each discovered API version
                options.AddSwaggerEndpoint(prefix, description.GroupName);
            }
        }

        private static void AddSwaggerEndpoint(this SwaggerUIOptions options, string prefix, string groupName)
        {
            options.SwaggerEndpoint($"{prefix}swagger/{groupName}/swagger.json",
                $"{groupName.ToUpperInvariant()} Docs");
        }
    }
}