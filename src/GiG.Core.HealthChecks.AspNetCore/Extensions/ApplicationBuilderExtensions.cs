using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.AspNetCore.Extensions
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

            var options = builder.ApplicationServices.GetService<IOptions<HealthCheckOptions>>()?.Value ?? new HealthCheckOptions();
            
            var loggerFactory = builder.ApplicationServices.GetService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger("GiG.Core.HealthChecks");
            
            Task WriteLogAndJsonResponseWriter(HttpContext context, HealthReport report)
            {
                HealthCheckEndpointWriter.WriteUnHealthyLog(logger, report);
                return HealthCheckEndpointWriter.WriteJsonResponseWriter(context, report);
            }

            return builder
                .UseHealthChecks(options.ReadyUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.ReadyTag),
                    ResponseWriter = WriteLogAndJsonResponseWriter
                })
                .UseHealthChecks(options.LiveUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = check => check.Tags.Contains(Constants.LiveTag),
                    ResponseWriter = WriteLogAndJsonResponseWriter
                })
                .UseHealthChecks(options.CombinedUrl, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    ResponseWriter = WriteLogAndJsonResponseWriter
                });
        }
    }
}