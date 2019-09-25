using GiG.Core.Orleans.Hosting.Silo.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace GiG.Core.Orleans.Hosting.Silo.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        private const string ClusterOptionsDefaultSection = "Orleans:Cluster";

        /// <summary>
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/> array which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies([NotNull] this ISiloBuilder builder, [NotNull] params Assembly[] assemblies)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            
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
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="types">The <see cref="Type"/> array which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies([NotNull] this ISiloBuilder builder, [NotNull] params Type[] types)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (types == null) throw new ArgumentNullException(nameof(types));
            
            builder.ConfigureApplicationParts(parts =>
            {
                foreach (var type in types)
                {
                    parts.AddApplicationPart(type.Assembly).WithReferences();
                }
            });

            return builder;
        }

        /// <summary>
        /// Configures the Orleans Cluster using a given Configuration section.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));
            
            builder.Configure<ClusterOptions>(configurationSection);

            return builder;
        }

        /// <summary>
        /// Configures the Orleans Cluster. Will retrieve configuration from the default Configuration Section.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
  
            var configurationSection = configuration.GetSection(ClusterOptionsDefaultSection);
            if (configurationSection == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{ClusterOptionsDefaultSection}' does not exist");
            }

            return ConfigureCluster(builder, configurationSection);
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="siloPort">Silo port which are default set to the default provided by in endpoint options.</param>
        /// <param name="gatewayPort">Gateway port which are default set to the default provided by endpoint options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoints([NotNull] this ISiloBuilder builder, int siloPort = EndpointOptions.DEFAULT_SILO_PORT, int gatewayPort = EndpointOptions.DEFAULT_GATEWAY_PORT)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            var siloAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                   .First(a => a.AddressFamily == AddressFamily.InterNetwork);

            return builder.Configure((EndpointOptions options) =>
            {
                options.AdvertisedIPAddress = siloAddress;
                options.SiloPort = siloPort;
                options.GatewayPort = gatewayPort;
            });
        }

        /// <summary>
        /// Configures the Orleans Dashboard.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> from which to bind to <see cref="DashboardOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDashboard([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
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

        /// <summary>
        /// Configures the Silo Builder with default configurations.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDefaults([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            
            return builder
                .ConfigureCluster(configuration)
                .ConfigureEndpoints()
                .ConfigureDashboard(configuration);
        }
    }
}