using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
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
                    if (healthReport.Status != HealthStatus.Healthy)
                    {
                        WriteDetails(writer, healthReport.Entries.Where(x => x.Value.Status != HealthStatus.Healthy));
                    }
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

            // Added missing logs when Health Check fails - https://github.com/dotnet/extensions/issues/2716
            foreach (var entry in healthReport.Entries.Where(x => x.Value.Status != HealthStatus.Healthy))
            {
                logger.LogWarning(entry.Value.Exception, "Health check {HealthCheckName} was {HealthCheckStatus} threw an exception {HealthCheckException}",
                    entry.Key, entry.Value.Status,  entry.Value.Exception?.Message);
            }
        }
        
        private static void WriteDetails(Utf8JsonWriter writer, IEnumerable<KeyValuePair<string, HealthReportEntry>> entries)
        {
            writer.WriteStartObject("details");
            foreach (var entry in entries)
            {
                writer.WriteStartObject(entry.Key);
                writer.WriteString("status", entry.Value.Status.ToString());
                writer.WriteString("details", entry.Value.Description);
                if (entry.Value.Exception?.Message != null)
                {
                    writer.WriteString("exception", entry.Value.Exception.Message);
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}