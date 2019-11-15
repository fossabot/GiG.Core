using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.HealthChecks.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Ready and Live HealthCheck Endpoints.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseHealthChecks([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = builder.ApplicationServices
                              .GetService<IOptions<HealthChecksOptions>>()?.Value ?? new HealthChecksOptions();

            return builder
                .UseHealthChecks(options.ReadyUrl, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                })
                .UseHealthChecks(options.LiveUrl, new HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                })
                .UseHealthChecks(options.CombinedUrl, new HealthCheckOptions
                {
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                });
        }
    }
}