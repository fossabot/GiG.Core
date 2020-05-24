using GiG.Core.Orleans.Silo.Dashboard.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans;

namespace GiG.Core.Orleans.Silo.Dashboard.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the Orleans Dashboard to start inside the web app host using the provided configuration.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        public static void UseDashboard(this IApplicationBuilder builder)
        {
            var options = builder.ApplicationServices.GetService<IOptions<DashboardOptions>>()?.Value ?? new DashboardOptions();
            
            if (options.IsEnabled)
            {
                builder.UseOrleansDashboard(new OrleansDashboard.DashboardOptions
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