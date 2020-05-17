using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Streams;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using ClusterMembershipOptions = Orleans.Configuration.ClusterMembershipOptions;

namespace GiG.Core.Orleans.Silo.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Adds Assemblies to Silo Builder with references.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="assemblies">The <see cref="Assembly"/> array which will be added to the Silo.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder AddAssemblies([NotNull] this ISiloBuilder builder,
            [NotNull] params Assembly[] assemblies)
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
        /// Configures the Orleans Cluster.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="ClusterOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder builder,
            [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true)
                throw new ConfigurationErrorsException(
                    $"Configuration section '{configurationSection?.Path}' is incorrect.");

            var options = configurationSection.Get<Clustering.Abstractions.ClusterMembershipOptions>();

            builder.Configure<ClusterOptions>(configurationSection);

            builder.Configure<ClusterMembershipOptions>(x =>
            {
                x.DefunctSiloExpiration = options.DefunctSiloExpiration;
                x.DefunctSiloCleanupPeriod = options.DefunctSiloCleanupPeriod;
            });

            return builder;
        }

        /// <summary>
        /// Configures the Orleans Cluster. Will retrieve configuration from the default Configuration Section.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="ClusterOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureCluster([NotNull] this ISiloBuilder builder,
            [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return ConfigureCluster(builder, configuration.GetSection(Constants.ClusterOptionsDefaultSectionName));
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="EndpointOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoints([NotNull] this ISiloBuilder builder,
            [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.ConfigureEndpoints(configuration.GetSection(Constants.EndpointOptionsDefaultSectionName));
        }

        /// <summary>
        /// Configures the Silo's endpoint.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="EndpointOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureEndpoints([NotNull] this ISiloBuilder builder,
            IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var endpointOptions = configurationSection?.Get<EndpointOptions>() ?? new EndpointOptions();

            var siloAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList
                .First(a => a.AddressFamily == AddressFamily.InterNetwork);

            return builder.Configure((EndpointOptions options) =>
            {
                options.AdvertisedIPAddress = siloAddress;
                options.SiloPort = endpointOptions.SiloPort;
                options.GatewayPort = endpointOptions.GatewayPort;
            });
        }

        /// <summary>
        /// Configures the Silo Builder with default configurations.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="ClusterOptions"/> and <see cref="EndpointOptions"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder ConfigureDefaults([NotNull] this ISiloBuilder builder,
            [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder
                .ConfigureCluster(configuration)
                .ConfigureEndpoints(configuration);
        }

        /// <summary>
        /// Configures the Silo PubSub Type for Stream Provider.
        /// </summary>
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="streamProviderName">The Stream Provider name.</param>
        /// <param name="pubSubType">The PubSub Type.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static ISiloBuilder ConfigurePubSubType([NotNull] this ISiloBuilder builder, [NotNull] string streamProviderName, StreamPubSubType pubSubType)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrEmpty(streamProviderName)) throw new ArgumentException($"'{nameof(streamProviderName)}' must not be null, empty or whitespace.", nameof(streamProviderName));

            return builder.ConfigureServices(x =>
                x.Configure<StreamPubSubOptions>(streamProviderName, 
                    o => o.PubSubType = pubSubType));
        }
    }
}