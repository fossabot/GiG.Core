using GiG.Core.Web.Docs.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;
using System.Text;

namespace GiG.Core.Web.Docs.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Documentation to API.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseApiDocs([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices.GetService<IOptions<ApiDocsOptions>>()?.Value;
            if (options == null) throw new ConfigurationErrorsException("ConfigureApiDocs need to be registered");

            if (!options.IsEnabled)
            {
                return app;
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

            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.ShowExtensions();
                    c.RoutePrefix = options.Url;
                    c.SwaggerEndpoint($"{endpointPrefix}swagger/v1/swagger.json", "V1 Docs");
                    c.DisplayRequestDuration();
                });
        }
    }
}