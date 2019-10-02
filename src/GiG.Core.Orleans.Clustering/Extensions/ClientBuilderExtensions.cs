﻿using JetBrains.Annotations;
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
        /// Register Membership Provider From Configuration 
        /// </summary>       
        /// <param name="builder">The Orleans <see cref="IClientBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Membership provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure membership providers of type <see cref="IClientBuilder"/>.</param>
        /// <returns>Returns the <see cref="IClientBuilder"/> so that more methods can be chained.</returns>
        public static IClientBuilder UseMembershipProvider([NotNull] this IClientBuilder builder, [NotNull] IConfiguration configuration, [NotNull] Action<MembershipProviderBuilder<IClientBuilder>> configureProvider)
        {
            return builder.UseMembershipProviderInternal(configuration, configureProvider);
        }
    }
}