using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans;
using System;

namespace GiG.Core.Orleans.Clustering.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Register Membership Provider From Configuration.
        /// </summary>       
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains the Membership Provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure Membership Providers of type <see cref="IClientBuilder"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder UseMembershipProvider([NotNull] this IClientBuilder builder, [NotNull] IConfiguration configuration, [NotNull] Action<MembershipProviderBuilder<IClientBuilder>> configureProvider)
        {
            return builder.UseMembershipProviderInternal(configuration, configureProvider);
        }

        /// <summary>
        /// Register Membership Provider From Configuration Section.
        /// </summary>       
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains the Membership Provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure Membership Providers of type <see cref="IClientBuilder"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder UseMembershipProvider([NotNull] this IClientBuilder builder, [NotNull] IConfigurationSection configurationSection, [NotNull] Action<MembershipProviderBuilder<IClientBuilder>> configureProvider)
        {
            return builder.UseMembershipProviderInternal(configurationSection, configureProvider);
        }
    }
}