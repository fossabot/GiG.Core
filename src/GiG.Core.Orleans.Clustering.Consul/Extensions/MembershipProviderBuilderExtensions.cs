using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Consul.Extensions
{
    /// <summary>
    /// Membership Provider Builder Extensions.
    /// </summary>
    public static class MembershipProviderBuilderExtensions
    {
        private const string ProviderName = "Consul";
        
        /// <summary>
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="IClientBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Consul options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureConsulClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.RegisterProvider(ProviderName, x => x.ConfigureConsulClustering(configuration));

            return builder;
        }

        /// <summary>
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="IClientBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Consul options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type<see cref="IClientBuilder" />.</returns>
        public static MembershipProviderBuilder<IClientBuilder> ConfigureConsulClustering([NotNull] this MembershipProviderBuilder<IClientBuilder> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            builder.RegisterProvider(ProviderName, x => x.ConfigureConsulClustering(configurationSection));

            return builder;
        }
        
        /// <summary>
        /// Configures Consul in Silo.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="ISiloBuilder" />.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Consul options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureConsulClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> builder, [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builder.RegisterProvider(ProviderName, x => x.ConfigureConsulClustering(configuration));

            return builder;
        }

        /// <summary>
        /// Configures Consul in Orleans.
        /// </summary>
        /// <param name="builder">Membership Provider builder of type <see cref="ISiloBuilder" />.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Consul options.</param>
        /// <returns>Returns the MembershipProviderBuilder of type <see cref="ISiloBuilder" />.</returns>
        public static MembershipProviderBuilder<ISiloBuilder> ConfigureConsulClustering([NotNull] this MembershipProviderBuilder<ISiloBuilder> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection == null) throw new ArgumentNullException(nameof(configurationSection));

            builder.RegisterProvider(ProviderName, x => x.ConfigureConsulClustering(configurationSection));

            return builder;
        }

    }
}
