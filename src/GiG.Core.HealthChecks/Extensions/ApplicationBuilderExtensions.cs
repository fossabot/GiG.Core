using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using HealthCheckOptions = GiG.Core.HealthChecks.Abstractions.HealthCheckOptions;

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
                              .GetService<IOptions<HealthCheckOptions>>()?.Value ?? new HealthCheckOptions();

            return builder
                .UseHealthChecks(options.ReadyUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                })
                .UseHealthChecks(options.LiveUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                })
                .UseHealthChecks(options.CombinedUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = HealthCheckEndpointWriter.WriteJsonResponseWriter
                });
        }
    }
}