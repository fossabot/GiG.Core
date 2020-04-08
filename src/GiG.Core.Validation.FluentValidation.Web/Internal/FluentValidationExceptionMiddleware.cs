using FluentValidation;
using FluentValidation.Results;
using GiG.Core.Models;
using GiG.Core.Validation.FluentValidation.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace GiG.Core.Validation.FluentValidation.Web.Internal
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
            context.Response.ContentType = Constants.ProblemJsonMimeType;

            var errorResponse = BuildValidationResponse(ex.Errors);

            _logger.LogInformation(Constants.GenericValidationErrorMessage, ex);
            var json = errorResponse.Serialize(_jsonOptionsAccessor?.Value?.Encoder);

            await context.Response.WriteAsync(json);
        }

        private static ErrorResponse BuildValidationResponse(IEnumerable<ValidationFailure> errors)
        {
            var errorMessageDictionary = new Dictionary<string, List<string>>();

            foreach (var error in errors)
            {
                if (errorMessageDictionary.ContainsKey(error.PropertyName))
                {
                    errorMessageDictionary[error.PropertyName].Add(error.ErrorMessage);
                }
                else
                {
                    errorMessageDictionary.Add(error.PropertyName, new List<string> { error.ErrorMessage });
                }
            }

            var errorResponse = new ErrorResponse
            {
                Errors = errorMessageDictionary
            };

            return errorResponse;
        }
    }
}