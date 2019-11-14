using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

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
        public static List<IEndpointConventionBuilder> MapHealthChecks([NotNull] this IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null) throw new ArgumentNullException(nameof(endpointRouteBuilder));

            var options = endpointRouteBuilder.ServiceProvider.GetService<IOptions<HealthChecksOptions>>()?.Value ?? new HealthChecksOptions();

            var conventionBuilders = new List<IEndpointConventionBuilder>
            {
                endpointRouteBuilder.MapHealthChecks(options.ReadyUrl, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                }),
                endpointRouteBuilder.MapHealthChecks(options.LiveUrl, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                }),
                endpointRouteBuilder.MapHealthChecks(options.CombinedUrl, new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                })
            };
            
            return conventionBuilders;
        }
    }
}
