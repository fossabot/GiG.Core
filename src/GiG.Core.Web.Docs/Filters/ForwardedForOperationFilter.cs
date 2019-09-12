using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace GiG.Core.Web.Docs.Filters
{
    /// <summary>
    /// ForwardedForOperationFilter to add X-Forwarded-For In Api Docs.
    /// </summary>
    internal class ForwardedForOperationFilter : IOperationFilter
    {
        private readonly ForwardedHeadersOptions _forwardedHeadersOptions;

        public ForwardedForOperationFilter(IOptions<ForwardedHeadersOptions> forwardedHeadersOptions)
        {
            _forwardedHeadersOptions = forwardedHeadersOptions.Value;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = _forwardedHeadersOptions?.ForwardedForHeaderName ??
                       ForwardedHeadersDefaults.XForwardedForHeaderName,
                Description = "Remote IP Address",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
