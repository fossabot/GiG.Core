using GiG.Core.Orleans.Silo.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans;

namespace GiG.Core.Orleans.Silo.Dashboard.Extensions
{
    /// <summary>
    /// Application Builder Extensions for Orleans Dashboard.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the Orleans Dashboard to start inside the web app host using the provided configuration.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> on which to configure the dashboard.</param>
        public static void UseDashboard(this IApplicationBuilder applicationBuilder)
        {
            var options = applicationBuilder.ApplicationServices.GetService<IOptions<DashboardOptions>>()?.Value ?? new DashboardOptions();
            
            if (options.IsEnabled)
            {
                applicationBuilder.UseOrleansDashboard(new OrleansDashboard.DashboardOptions
                {
                    HostSelf = options.HostSelf,
                    Port = options.Port,
                    BasePath = options.Path,
                    Username = options.Username,
                    Password = options.Password,
                    HideTrace = options.HideTrace,
                    CounterUpdateIntervalMs = options.CounterUpdateIntervalMs
                });    
            }
        }
    }
}