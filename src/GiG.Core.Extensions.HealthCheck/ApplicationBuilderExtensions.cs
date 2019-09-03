using GiG.Core.Abstractions.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace GiG.Core.Extensions.HealthCheck
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Ready and Live HealthCheck endpoints
        /// The readiness check uses all registered checks with the 'ready' tag.
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder.</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder.</returns>
        public static IApplicationBuilder UseHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health/ready", new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                Predicate = (check) => check.Tags.Contains(Constants.ReadyTag),
            });

            app.UseHealthChecks("/health/live", new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false
            });

            return app;
        }
    }
}
