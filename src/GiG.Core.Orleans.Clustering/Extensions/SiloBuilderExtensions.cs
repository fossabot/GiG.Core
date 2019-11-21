using GiG.Core.Orleans.Clustering.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Register Membership Provider From Configuration.
        /// </summary>       
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains the Membership Provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure Membership Providers of type <see cref="ISiloBuilder"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder UseMembershipProvider([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration, [NotNull] Action<MembershipProviderBuilder<ISiloBuilder>> configureProvider)
        {           
            return builder.UseMembershipProviderInternal(configuration, configureProvider);           
        }

        /// <summary>
        /// Register Membership Provider From Configuration Section.
        /// </summary>       
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains the Membership Provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure Membership Providers of type <see cref="ISiloBuilder"/>.</param>
        /// <returns>The <see cref="ISiloBuilder"/>.</returns>
        public static ISiloBuilder UseMembershipProvider([NotNull] this ISiloBuilder builder, [NotNull] IConfigurationSection configurationSection, [NotNull] Action<MembershipProviderBuilder<ISiloBuilder>> configureProvider)
        {
            return builder.UseMembershipProviderInternal(configurationSection, configureProvider);
        }
    }
}