using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.Validation.FluentValidation.Web.Internal
{
    internal class FluentValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<JsonSerializerOptions> _jsonOptionsAccessor;

        public FluentValidationExceptionMiddleware(
            RequestDelegate next,
            IOptions<JsonSerializerOptions> jsonOptionsAccessor)
        {
            _next = next;
            _jsonOptionsAccessor = jsonOptionsAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, _jsonOptionsAccessor?.Value?.Encoder, ex);
            }
        }

        private static async ValueTask HandleValidationExceptionAsync(HttpContext context, JavaScriptEncoder javaScriptEncoder, ValidationException ex)
        {
            context.Response.StatusCode = Constants.StatusCode;
            context.Response.ContentType = Constants.ProblemJsonMimeType;

            var errors = ConvertToDictionary(ex.Errors);
            await SerialiseErrorResponseAsync(context.Response, javaScriptEncoder, errors);
        }

        private static async ValueTask SerialiseErrorResponseAsync(HttpResponse response, JavaScriptEncoder javaScriptEncoder, Dictionary<string, List<string>> errors)
        {
            var writerOptions = new JsonWriterOptions {Encoder = javaScriptEncoder ?? JavaScriptEncoder.UnsafeRelaxedJsonEscaping};
            using (var writer = new Utf8JsonWriter(response.Body, writerOptions))
            {
                writer.WriteStartObject();
                writer.WriteString("errorSummary", "One or more validation errors occurred.");
                writer.WriteStartObject("errors");
                WriteErrorsArray(writer, errors);
                writer.WriteEndObject();
                writer.WriteEndObject();
                
                await writer.FlushAsync();
            }
        }

        private static void WriteErrorsArray(Utf8JsonWriter writer, Dictionary<string, List<string>> errors)
        {
            foreach (var error in errors)
            {
                writer.WriteStartArray(error.Key);

                foreach (var errorMessage in error.Value)
                {
                    writer.WriteStringValue(errorMessage);
                }

                writer.WriteEndArray();
            }
        }

        private static Dictionary<string, List<string>> ConvertToDictionary(IEnumerable<ValidationFailure> validationFailures)
        {
            var dictionary = new Dictionary<string, List<string>>();
            foreach (var errors in validationFailures)
            {
                if (dictionary.ContainsKey(errors.PropertyName))
                {
                    dictionary[errors.PropertyName].Add(errors.ErrorMessage);
                }
                else
                {
                    dictionary.Add(errors.PropertyName, new List<string> {errors.ErrorMessage});
                }
            }

            return dictionary;
        }
    }
}