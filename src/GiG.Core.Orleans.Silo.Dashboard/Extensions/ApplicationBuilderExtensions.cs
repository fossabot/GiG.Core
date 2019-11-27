﻿using GiG.Core.Orleans.Silo.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orleans;

namespace GiG.Core.Orleans.Silo.Dashboard.Extensions
{
    /// <summary>
    /// Application Builder Extensions for Orleans Dashboard
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures the Orleans Dashboard to start inside the web app host using the provided configuration.
        /// </summary>
        /// <param name="applicationBuilder">The <see cref="IApplicationBuilder"/> on which to configure the dashboard.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from which to bind the dashboard options.</param>
        public static void UseDashboard(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            var options = configuration.GetSection(DashboardOptions.DefaultSectionName).Get<DashboardOptions>();
            
            if (options.IsEnabled)
            {
                applicationBuilder.UseOrleansDashboard(new OrleansDashboard.DashboardOptions
                {
                    HostSelf = options.HostSelf,
                    Port = options.Port,
                    BasePath = options.Path
                });    
            }
        }
    }
}