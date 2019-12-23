using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.Hosting
{
    /// <summary>
    /// Info Management Endpoint Writer.
    /// </summary>
    public static class InfoManagementWriter
    {
        /// <summary>
        /// Writes the Application Metadata in Json format.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/>.</param>
        /// <returns></returns>
        public static Task WriteJsonResponseWriter(HttpContext httpContext)
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
                    writer.WriteString(nameof(ApplicationMetadata.Checksum), ApplicationMetadata.Checksum);
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return httpContext.Response.WriteAsync(json);
            }
        }
    }
}
