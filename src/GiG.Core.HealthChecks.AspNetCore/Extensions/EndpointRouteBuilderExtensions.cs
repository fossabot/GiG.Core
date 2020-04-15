using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.AspNetCore.Extensions
{
    /// <summary>
    /// EndpointRouteBuilder Extensions.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds HealthCheck endpoints to the <see cref="IEndpointRouteBuilder "/>.
        /// </summary>
        /// <param name="endpointRouteBuilder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns>A list of <see cref="IEndpointConventionBuilder"/> that can be used to enrich the endpoints.</returns>
        public static HealthCheckEndpoints MapHealthChecks([NotNull] this IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null) throw new ArgumentNullException(nameof(endpointRouteBuilder));

            var healthCheckOptions = endpointRouteBuilder.ServiceProvider.GetService<IOptions<HealthCheckOptions>>()?.Value ?? new HealthCheckOptions();

            var loggerFactory = endpointRouteBuilder.ServiceProvider.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("GiG.Core.HealthChecks");
            
            Task WriteLogAndJsonResponseWriter(HttpContext context, HealthReport report)
            {
                HealthCheckEndpointWriter.WriteUnHealthyLog(logger, report);
                return HealthCheckEndpointWriter.WriteJsonResponseWriter(context, report);
            }

            return new HealthCheckEndpoints
            {
                Ready = endpointRouteBuilder.MapHealthChecks(healthCheckOptions.ReadyUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = WriteLogAndJsonResponseWriter
                }),
                Live = endpointRouteBuilder.MapHealthChecks(healthCheckOptions.LiveUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = WriteLogAndJsonResponseWriter
                }),
                Combined = endpointRouteBuilder.MapHealthChecks(healthCheckOptions.CombinedUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = WriteLogAndJsonResponseWriter
                })
            };
        }
    }
}
