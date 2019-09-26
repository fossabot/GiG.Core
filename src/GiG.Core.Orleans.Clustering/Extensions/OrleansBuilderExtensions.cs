using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Clustering.Extensions
{
    /// <summary>
    /// Configures builders extensions for orleans membership providers.
    /// </summary>
    public static class OrleansBuilderExtensions
    {
        /// <summary>
        /// Register Membership Provider From Configuration.
        /// </summary>
        /// <typeparam name="T">Generic to represent host interfaces used for orleans start up.</typeparam>
        /// <param name="builder">Builder of type T used in orleans start up.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Membership provider's configuration options.</param>
        /// <param name="configureProvider">Action used to configure membership providers.</param>
        /// <returns></returns>
        internal static T UseMembershipProviderInternal<T>([NotNull] this T builder, [NotNull] IConfiguration configuration, [NotNull] Action<MembershipProviderBuilder<T>> configureProvider)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (configureProvider == null) throw new ArgumentNullException(nameof(configureProvider));

            var membershipProviderOptions = configuration.GetSection(MembershipProviderOptions.DefaultSectionName).Get<MembershipProviderOptions>();            
            if (membershipProviderOptions == null || string.IsNullOrWhiteSpace(membershipProviderOptions.Name))
            {
                throw new ConfigurationErrorsException($"No Orleans Membership Provider was specified in the configuration section {MembershipProviderOptions.DefaultSectionName}");
            }

            var membershipBuilder = new MembershipProviderBuilder<T>(builder, membershipProviderOptions.Name);
            configureProvider(membershipBuilder);

            if (!membershipBuilder.IsRegistered)
            {
                throw new ConfigurationErrorsException($"No Orleans Membership Providers were registered from the configuration section {MembershipProviderOptions.DefaultSectionName}");
            }

            return builder;
        }
    }
}
