using GiG.Core.Web.FluentValidation.Internal;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace GiG.Core.Web.FluentValidation.Extensions
{
    internal static class ValidationResponseExtensions
    {
        internal static string Serialize(this ValidationResponse validationResponse, JavaScriptEncoder javaScriptEncoder)
        {
            using (var stream = new MemoryStream())
            {
                var writerOptions = new JsonWriterOptions {Encoder = javaScriptEncoder ?? JavaScriptEncoder.UnsafeRelaxedJsonEscaping};

                using (var writer = new Utf8JsonWriter(stream, writerOptions))
                {
                    writer.WriteStartObject();
                    writer.WriteString("title", validationResponse.Title);
                    writer.WriteNumber("status", validationResponse.Status);
                    writer.WriteStartObject("errors");
                    validationResponse.Errors.ToList().ForEach(kvp =>
                    {
                        writer.WriteStartArray(kvp.Key);
                        kvp.Value.ForEach(x => writer.WriteStringValue(x));
                        writer.WriteEndArray();
                    });

                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}
