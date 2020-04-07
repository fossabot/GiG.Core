using GiG.Core.Models;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

[assembly: InternalsVisibleTo("GiG.Core.Benchmarks")]

namespace GiG.Core.Validation.FluentValidation.Web.Extensions
{
    internal static class ValidationResponseExtensions
    {
        internal static string Serialize(this ErrorResponse errorResponse, JavaScriptEncoder javaScriptEncoder)
        {
            using (var stream = new MemoryStream())
            {
                var writerOptions = new JsonWriterOptions {Encoder = javaScriptEncoder ?? JavaScriptEncoder.UnsafeRelaxedJsonEscaping};

                using (var writer = new Utf8JsonWriter(stream, writerOptions))
                {
                    writer.WriteStartObject();
                    writer.WriteString("errorSummary", errorResponse.ErrorSummary);
                    writer.WriteStartObject("errors");
                    errorResponse.Errors.ToList().ForEach(kvp =>
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