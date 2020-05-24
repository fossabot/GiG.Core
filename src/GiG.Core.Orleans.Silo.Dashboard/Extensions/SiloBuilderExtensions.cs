using GiG.Core.Orleans.Silo.Dashboard.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Silo.Dashboard.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configures the Orleans Dashboard.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="DashboardOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDashboard([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var configurationSection = configuration.GetSection(DashboardOptions.DefaultSectionName);
            builder.ConfigureServices(x => x.Configure<DashboardOptions>(configurationSection));

            var dashboardOptions = configurationSection.Get<DashboardOptions>() ?? new  DashboardOptions();
            if (dashboardOptions.IsEnabled)
            {
                builder.UseDashboard(options =>
                {
                    options.BasePath = dashboardOptions.Path;
                    options.Port = dashboardOptions.Port;
                    options.HostSelf = dashboardOptions.HostSelf;
                    options.Username = options.Username;
                    options.Password = options.Password;
                    options.HideTrace = options.HideTrace;
                    options.CounterUpdateIntervalMs = options.CounterUpdateIntervalMs;
                });
            }

            return builder;
        }
    }
}