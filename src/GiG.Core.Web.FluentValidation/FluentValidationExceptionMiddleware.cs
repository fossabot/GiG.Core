using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.Web.FluentValidation
{
    internal class FluentValidationExceptionMiddleware
    {
        private const string GenericValidationErrorMessage = "Fluent Validation exception";
        private const string Json = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly IOptions<JsonSerializerOptions> _jsonOptionsAccessor;

        public FluentValidationExceptionMiddleware(
            RequestDelegate next,
            ILogger<FluentValidationExceptionMiddleware> logger,
            IOptions<JsonSerializerOptions> jsonOptionsAccessor)
        {
            _next = next;
            _logger = logger;
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
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = Json;

            var responseDictionary = BuildResponseDictionary(ex.Errors);

            _logger.LogInformation(GenericValidationErrorMessage, ex);

            await context.Response.WriteAsync(JsonSerializer.Serialize(responseDictionary,
                _jsonOptionsAccessor.Value));
        }

        private static IDictionary<string, List<string>> BuildResponseDictionary(IEnumerable<ValidationFailure> errors)
        {
            var dictionary = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                if (dictionary.ContainsKey(error.PropertyName))
                {
                    dictionary[error.PropertyName].Add(error.ErrorMessage);
                }
                else
                {
                    dictionary.Add(error.PropertyName, new List<string> { error.ErrorMessage });
                }
            }

            return dictionary;
        }
    }
}
