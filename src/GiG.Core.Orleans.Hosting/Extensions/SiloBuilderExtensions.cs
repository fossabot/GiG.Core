using GiG.Core.Orleans.Abstractions.Configuration;
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
        private const string DashboardOptionsDefaultSection = "Orleans:Dashboard";

        /// <summary>
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
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
        /// <param name="builder">The Orleans Silo Builder.</param>
        /// <param name="configurationSection">The configuration section containing the Cluster options.</param>
        public static ISiloBuilder ConfigureCluster(this ISiloBuilder builder, IConfigurationSection configurationSection)
        {
            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }

        /// <summary>
        /// Configures the Orleans Cluster. Will retrieve configuration from the default Configuration Section.
        /// </summary>
        public static ISiloBuilder ConfigureCluster(this ISiloBuilder builder, IConfiguration configuration)
        {
            var configurationSection = configuration.GetSection(ClusterOptionsDefaultSection);
            if (configurationSection == null)
            {
                throw new  InvalidOperationException($"Configuration section '{ClusterOptionsDefaultSection}' does not exist");
            }

            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
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
        public static ISiloBuilder ConfigureDashboard(this ISiloBuilder builder, IConfiguration configuration)
        {

            var orleansConfiguration = configuration.GetSection(DashboardOptionsDefaultSection).Get<DashboardOptions>();
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
        public static ISiloBuilder ConfigureDefaults(this ISiloBuilder builder, IConfiguration configuration)
        {
            return builder
                .ConfigureCluster(configuration)
                .ConfigureEndpoint()
                .ConfigureDashboard(configuration);
        }
    }
}