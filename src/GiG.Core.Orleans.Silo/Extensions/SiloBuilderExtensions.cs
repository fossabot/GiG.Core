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
using SiloOptions = GiG.Core.Orleans.Silo.Abstractions.SiloOptions;

namespace GiG.Core.Orleans.Silo.Extensions
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
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/> array which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies([NotNull] this ISiloBuilder siloBuilder, [NotNull] params Assembly[] assemblies)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            siloBuilder.ConfigureApplicationParts(parts =>
            {
                foreach (var assembly in assemblies)
                {
                    parts.AddApplicationPart(assembly).WithReferences();
                }
            });

            return siloBuilder;
        }

        /// <summary>
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="types">The <see cref="Type"/> array which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies([NotNull] this ISiloBuilder siloBuilder, [NotNull] params Type[] types)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (types == null) throw new ArgumentNullException(nameof(types));

            siloBuilder.ConfigureApplicationParts(parts =>
            {
                foreach (var type in types)
                {
                    parts.AddApplicationPart(type.Assembly).WithReferences();
                }
            });

            return siloBuilder;
        }

        /// <summary>
        /// Configures the Orleans Cluster using a given Configuration section.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            siloBuilder.Configure<ClusterOptions>(configurationSection);

            return siloBuilder;
        }

        /// <summary>
        /// Configures the Orleans Cluster. Will retrieve configuration from the default Configuration Section.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Cluster options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return ConfigureCluster(siloBuilder, configuration.GetSection(ClusterOptionsDefaultSection));
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Silo options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoints([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return siloBuilder.ConfigureEndpoints(configuration.GetSection(SiloOptions.DefaultSectionName));
        }
        
        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> containing the Silo options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoints([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfigurationSection configurationSection)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
         
            var siloOptions = configurationSection?.Get<SiloOptions>() ?? new SiloOptions();
          
            var siloAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .First(a => a.AddressFamily == AddressFamily.InterNetwork);

            return siloBuilder.Configure((EndpointOptions options) =>
            {
                options.AdvertisedIPAddress = siloAddress;
                options.SiloPort = siloOptions.SiloPort;
                options.GatewayPort = siloOptions.GatewayPort;
            });
        }

        /// <summary>
        /// Configures the Silo Builder with default configurations.
        /// </summary>
        /// <param name="siloBuilder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> containing the Cluster and Silo options.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDefaults([NotNull] this ISiloBuilder siloBuilder, [NotNull] IConfiguration configuration)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return siloBuilder
                .ConfigureCluster(configuration)
                .ConfigureEndpoints(configuration);
        }
    }
}