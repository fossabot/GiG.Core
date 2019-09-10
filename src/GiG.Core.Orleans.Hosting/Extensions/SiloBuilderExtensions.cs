﻿using GiG.Core.Orleans.Abstractions.Configuration;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Reflection;

namespace GiG.Core.Orleans.Hosting.Extensions
{
    public static class SiloBuilderExtensions
    {
        private const string ClusterOptionsDefaultSection = "Orleans:Cluster";


        /// <summary>
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="assemblies">The Assemblies which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies(this ISiloBuilder builder,
            params Assembly[] assemblies)
        {
            builder.ConfigureApplicationParts(parts =>
            {
                foreach (var assembly in assemblies)
                {
                    parts.AddApplicationPart(assembly).WithReferences();
                }
            });

            return builder;
        }

        /// <summary>
        /// Configures the Orleans Cluster using a given Configuration section.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The configuration section containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster(this ISiloBuilder builder, IConfigurationSection configurationSection)
        {
            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }


        /// <summary>
        /// Configures the Orleans Cluster. Will retrieve configuration from the default Configuration Section.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration"></param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster(this ISiloBuilder builder, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentException($"Configuration is null", nameof(configuration));
            }

            var configurationSection = configuration.GetSection(ClusterOptionsDefaultSection);
            if (configurationSection == null)
            {
                throw new  InvalidOperationException($"Configuration section '{ClusterOptionsDefaultSection}' does not exist");
            }

            return ConfigureCluster(builder, configurationSection);
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoint(this ISiloBuilder builder)
        {
            return builder.Configure((EndpointOptions options) =>
            {
                options.AdvertisedIPAddress = IPAddress.Loopback;
            });
        }


        /// <summary>
        /// Configures the Orleans Dashboard.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration"></param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDashboard(this ISiloBuilder builder, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentException($"Configuration is null", nameof(configuration));
            }

            var orleansConfiguration = configuration.GetSection(DashboardOptions.DefaultConfigurationSection).Get<DashboardOptions>();
            if (orleansConfiguration?.Enabled ?? false)
            {
                builder.UseDashboard(options =>
                {
                    options.BasePath = orleansConfiguration.Path;
                    options.Port = orleansConfiguration.Port;
                });
            }

            return builder;
        }


        /// <summary>
        /// Configures the Silo Builder with default configurations.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration"></param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDefaults(this ISiloBuilder builder, IConfiguration configuration)
        {
            return builder
                .ConfigureCluster(configuration)
                .ConfigureEndpoint()
                .ConfigureDashboard(configuration);
        }
    }
}