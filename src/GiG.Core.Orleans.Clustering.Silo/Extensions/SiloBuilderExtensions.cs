using GiG.Core.Orleans.Clustering.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Hosting;
using System;

namespace GiG.Core.Orleans.Clustering.Silo.Extensions
{
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Register Membership Provider From Configuration.
        /// </summary>       
        /// <param name="builder">The Orleans <see cref="ISiloBuilder"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Membership provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure membership providers of type <see cref="ISiloBuilder"/>.</param>
        /// <returns>Returns the <see cref="ISiloBuilder"/> so that more methods can be chained.</returns>
        public static ISiloBuilder UseMembershipProvider([NotNull] this ISiloBuilder builder, [NotNull] IConfiguration configuration, [NotNull] Action<MembershipProviderBuilder<ISiloBuilder>> configureProvider)
        {           
            return builder.UseMembershipProviderInternal(configuration, configureProvider);           
        }
    }
}