using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Map the Info Management Endpoint to the Application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseInfoManagement([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices.GetService<IOptions<InfoManagementOptions>>()?.Value ?? new InfoManagementOptions();

            if (!options.IsEnabled) return app;

            return app.Map(options.Url, appBuilder =>
            {                    
                appBuilder.Run(WriteJsonResponseWriter);
            });
        }

        private static Task WriteJsonResponseWriter(HttpContext httpContext)
        {
            httpContext.Response.ContentType = "application/json";

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString(nameof(ApplicationMetadata.Name), ApplicationMetadata.Name);
                    writer.WriteString(nameof(ApplicationMetadata.Version), ApplicationMetadata.Version);
                    writer.WriteString(nameof(ApplicationMetadata.InformationalVersion), ApplicationMetadata.InformationalVersion);
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return httpContext.Response.WriteAsync(json);
            }
        }
    }
}