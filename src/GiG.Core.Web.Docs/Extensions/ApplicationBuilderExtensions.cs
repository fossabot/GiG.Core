using GiG.Core.Web.Docs.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Configuration;

namespace GiG.Core.Web.Docs.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Documentation To API.
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseApiDocs([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices.GetService<IOptions<ApiDocsOptions>>()?.Value;
            if (options == null) throw new ConfigurationErrorsException("ConfigureApiDocs need to be registered");

            if (!options.IsEnabled)
            {
                return app;
            }

            return app
                .UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.ShowExtensions();
                    c.RoutePrefix = options.Url;
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                    c.DisplayRequestDuration();
                });
        }
    }
}