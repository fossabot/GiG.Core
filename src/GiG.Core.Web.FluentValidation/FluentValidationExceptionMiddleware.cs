using FluentValidation;
using FluentValidation.Results;
using GiG.Core.Web.FluentValidation.Abstractions;
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
            context.Response.ContentType = Constants.JsonMimeType;

            var responseDictionary = BuildValidationResponse(ex.Errors);

            _logger.LogInformation(Constants.GenericValidationErrorMessage, ex);

            await context.Response.WriteAsync(JsonSerializer.Serialize(responseDictionary,
                _jsonOptionsAccessor.Value));
        }

        private static ValidationResponse BuildValidationResponse(IEnumerable<ValidationFailure> errors)
        {
            var errorMsgsDictionary = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                if (errorMsgsDictionary.ContainsKey(error.PropertyName))
                {
                    errorMsgsDictionary[error.PropertyName].Add(error.ErrorMessage);
                }
                else
                {
                    errorMsgsDictionary.Add(error.PropertyName, new List<string> { error.ErrorMessage });
                }
            }

            var validationResponse = new ValidationResponse
            {
                ValidationErrors = errorMsgsDictionary
            };

            return validationResponse;
        }
    }
}
