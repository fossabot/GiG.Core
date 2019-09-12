﻿using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Application Builder Extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Ready and Live HealthCheck endpoints
        /// </summary>
        /// <param name="app">The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</param>
        /// <returns>The <see cref="T:Microsoft.AspNetCore.Builder.IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseHealthChecks([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices
                              .GetService<IOptions<HealthChecksOptions>>()?.Value ??
                          new HealthChecksOptions();

            return app
                .UseHealthChecks(options.ReadyUrl, new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = WriteJsonResponseWriter
                })
                .UseHealthChecks(options.LiveUrl, new HealthCheckOptions()
                {
                    Predicate = (check) => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = WriteJsonResponseWriter
                })
                .UseHealthChecks(options.CombinedUrl, new HealthCheckOptions
                {
                    ResponseWriter = WriteJsonResponseWriter
                });
        }

        private static Task WriteJsonResponseWriter(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return httpContext.Response.WriteAsync(json);
            }
        }
    }
}