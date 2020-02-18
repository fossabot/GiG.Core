﻿using GiG.Core.Orleans.Silo.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Silo.Dashboard.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Configures the Orleans Dashboard.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from which to bind to <see cref="DashboardOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDashboard([NotNull] this ISiloBuilder siloBuilder,
            [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var configurationSection = configuration.GetSection(DashboardOptions.DefaultSectionName);
            siloBuilder.ConfigureServices(x => x.Configure<DashboardOptions>(configurationSection));

            var dashboardOptions = configurationSection.Get<DashboardOptions>() ?? new  DashboardOptions();
            if (dashboardOptions.IsEnabled)
            {
                siloBuilder.UseDashboard(options =>
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

            return siloBuilder;
        }
    }
}