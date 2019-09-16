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
    /// Application Builder Extensions for Info Management Endpoint.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Map the Info Management Endpoint to the Application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> on which to add the Info Management</param>
        /// <returns>The <see cref="IApplicationBuilder"/> so that more methods can be chained</returns>
        public static IApplicationBuilder UseInfoManagement([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices.GetService<IOptions<InfoManagementOptions>>()?.Value ?? new InfoManagementOptions();
            if (options.IsEnabled)
            {
                app.Map(options.Url, appBuilder =>
                {
                    var applicationMetadataAccessor =
                        appBuilder.ApplicationServices.GetRequiredService<IApplicationMetadataAccessor>();

                    appBuilder.Run(async context =>
                    {
                        await WriteJsonResponseWriter(context, applicationMetadataAccessor);
                    });
                });
            }

            return app;
        }

        private static Task WriteJsonResponseWriter(HttpContext httpContext, IApplicationMetadataAccessor accessor)
        {
            httpContext.Response.ContentType = "application/json";

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString(nameof(accessor.Name), accessor.Name);
                    writer.WriteString(nameof(accessor.Version), accessor.Version);
                    writer.WriteString(nameof(accessor.InformationalVersion), accessor.InformationalVersion);
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return httpContext.Response.WriteAsync(json);
            }
        }
    }
}