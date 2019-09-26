using JetBrains.Annotations;
using System;

namespace GiG.Core.Orleans.Clustering
{
    /// <summary>
    /// Membership provider builder which encapsulates an orleans builder support provider's registrations.
    /// </summary>
    /// <typeparam name="T"> Generic to represent host interfaces used for orleans start up.</typeparam>
    public class MembershipProviderBuilder<T>
    {
        private readonly T _builder;
        private readonly string _providerName;

        internal bool IsRegistered { get; private set; }

        internal MembershipProviderBuilder([NotNull] T builder, [NotNull] string providerName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            _providerName = providerName ?? throw new ArgumentNullException(nameof(providerName));
            _builder = builder;
        }

        /// <summary>
        /// Registers Membership Provider. 
        /// </summary>
        /// <param name="providerName"> Membership provider's name.</param>
        /// <param name="configureProvider">Action to support providers' builder extensions</param>
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
