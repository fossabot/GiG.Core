using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace GiG.Core.Extensions.HealthCheck
{
    /// <summary>
    /// Application Builder Extensions
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Ready and Live HealthCheck endpoints
        /// The readiness check uses all registered checks with the 'ready' tag.
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
        /// <param name="healthChecksOptions">The Health Check Options</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder.</returns>
        public static IApplicationBuilder UseHealthChecks([NotNull] this IApplicationBuilder app, [NotNull] HealthChecksOptions healthChecksOptions)
        {
            app.UseHealthChecks(healthChecksOptions.ReadyUrl, new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                Predicate = (check) => check.Tags.Contains(Constants.ReadyTag),
            });

            app.UseHealthChecks(healthChecksOptions.LiveUrl, new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false
            });

            return app;
        }
    }
}
