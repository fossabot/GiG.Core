﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks
{
    /// <summary>
    /// Health Check Endpoint Writer.
    /// </summary>
    public static class HealthCheckEndpointWriter
    {
        /// <summary>
        /// Writes the Health Check status in Json format.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
        /// <param name="healthReport">The <see cref="HealthReport"/>.</param>
        /// <returns></returns>
        public static Task WriteJsonResponseWriter(HttpContext httpContext, HealthReport healthReport)
        {
            httpContext.Response.ContentType = "application/json";

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", healthReport.Status.ToString());
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return httpContext.Response.WriteAsync(json);
            }
        }

        /// <summary>
        /// Writes a log when Health Check status is not Healthy.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger"/>.</param>
        /// <param name="healthReport">The <see cref="HealthReport"/>.</param>
        /// <returns></returns>
        public static void WriteUnHealthyLog(ILogger logger, HealthReport healthReport)
        {
            if (healthReport.Status == HealthStatus.Healthy || !logger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

            foreach (var entry in healthReport.Entries.Where(x => x.Value.Status != HealthStatus.Healthy))
            {
                logger.LogWarning(entry.Value.Exception, "Health check {HealthCheckName} was {HealthCheckStatus} threw an exception {HealthCheckException}",
                    entry.Key, entry.Value.Status,  entry.Value.Exception?.Message);
            }
        }
    }
}