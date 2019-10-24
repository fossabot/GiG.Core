using JetBrains.Annotations;
using System;

namespace GiG.Core.Orleans.Clustering.Abstractions
{
    /// <summary>
    /// Membership Provider Builder which encapsulates an Orleans Builder to support Membership Providers' Registrations.
    /// </summary>
    /// <typeparam name="T"> Generic to represent host interfaces used for Orleans start up.</typeparam>
    public class MembershipProviderBuilder<T>
    {
        private readonly T _builder;
        private readonly string _providerName;

        /// <summary>
        /// A value to indicate if the Membership Provider is registered or not.
        /// </summary>
        public bool IsRegistered { get; private set; }

        /// <summary>
        /// Constructor. 
        /// </summary>
        /// <param name="builder">Generic Provider</param>
        /// <param name="providerName">The Membership Provider's name.</param>
        public MembershipProviderBuilder([NotNull] T builder, [NotNull] string providerName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            _providerName = providerName ?? throw new ArgumentNullException(nameof(providerName));
            _builder = builder;
        }

        /// <summary>
        /// Registers Membership Provider. 
        /// </summary>
        /// <param name="providerName">The Membership Provider's name.</param>
        /// <param name="configureProvider">Action to support Membership Providers' builder extensions</param>
        public void RegisterProvider([NotNull] string providerName, [NotNull] Action<T> configureProvider)
        {
            if (providerName == null) throw new ArgumentNullException(nameof(providerName));
            if (configureProvider == null) throw new ArgumentNullException(nameof(configureProvider));

            if (!IsRegistered && _providerName.Equals(providerName))
            {
                configureProvider(_builder);
                IsRegistered = true;
            }
        }
    }
}