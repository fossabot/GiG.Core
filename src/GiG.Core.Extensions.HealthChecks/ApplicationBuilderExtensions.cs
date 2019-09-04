using GiG.Core.HealthChecks.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;

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
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder.</returns>
        public static IApplicationBuilder UseHealthChecks([NotNull] this IApplicationBuilder app, [NotNull] IConfiguration configuration)
        {
            var healthChecksSettings = new HealthChecksSettings();
            configuration?.GetSection(Constants.DefaultSectionName).Bind(healthChecksSettings);

            app.UseHealthChecks(healthChecksSettings.ReadyUrl, new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                Predicate = (check) => check.Tags.Contains(Constants.ReadyTag),
            });

            app.UseHealthChecks(healthChecksSettings.LiveUrl, new HealthCheckOptions()
            {
                AllowCachingResponses = true,
                // Exclude all checks and return a 200-Ok.
                Predicate = (_) => false
            });

            return app;
        }
    }
}
