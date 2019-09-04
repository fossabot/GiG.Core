using GiG.Core.HealthChecks.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Extensions.HealthCheck
{
    /// <summary>
    /// Service Collection Extensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the Memory Cache to the container.
        /// Adds the HealthCheckService to the container.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService
        ///     to.
        ///     </param>
        /// <param name="configuration">The Application Configuration</param>
        /// <returns>An instance of Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder
        ///     from which health checks can be registered.
        /// </returns>
        public static IHealthChecksBuilder AddCachedHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            return services.AddHealthChecks(configuration);
        }

        /// <summary>
        /// Adds the HealthCheckService to the container.
        /// </summary>
        /// <param name="services">The Microsoft.Extensions.DependencyInjection.IServiceCollection to add the Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckService
        ///     to.
        ///     </param>
        /// <param name="configuration">The Application Configuration</param>
        /// <returns>An instance of Microsoft.Extensions.DependencyInjection.IHealthChecksBuilder
        ///     from which health checks can be registered.
        /// </returns>
        public static IHealthChecksBuilder AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HealthChecksOptions>(configuration.GetSection(HealthChecksOptions.DefaultSectionName));
            return services.AddHealthChecks();
        }
    }
}
