﻿using GiG.Core.Orleans.Silo.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
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
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from which to bind to <see cref="DashboardOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDashboard([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
          
            var orleansConfiguration = configuration.GetSection(DashboardOptions.DefaultSectionName).Get<DashboardOptions>();
            
            if (orleansConfiguration?.IsEnabled ?? false)
            {
                builder.UseDashboard(options =>
                {
                    options.BasePath = orleansConfiguration.Path;
                    options.Port = orleansConfiguration.Port;
                });
            }

            return builder;
        }
    }
}