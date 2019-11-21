using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.Abstractions
{
    /// <summary>
    /// Tracing Configuration builder.
    /// </summary>
    public class TracingConfigurationBuilder
    {
        /// <summary>
        /// The Providers.
        /// </summary>
        public IDictionary<string, BasicProviderOptions> Providers { get; }

        /// <summary>
        /// The Service Collection.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Is Provider Configured
        /// </summary>
        public bool IsProviderConfigured { get; private set; }

        /// <summary>
        /// Tracing Configuration builder.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="providers">List of providers.</param>
        public TracingConfigurationBuilder([NotNull] IServiceCollection services, [NotNull] IDictionary<string, BasicProviderOptions> providers)
        {
            Providers = providers ?? throw new ArgumentNullException(nameof(providers));
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Register Tracing Provider.
        /// </summary>
        /// <param name="name">The Tracing Provider name.</param>
        /// <param name="tracingProvider">The <see cref="ITracingProvider"/>.</param>
        public TracingConfigurationBuilder RegisterProvider([NotNull] string name, [NotNull] ITracingProvider tracingProvider)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name));
            if (tracingProvider == null) throw new ArgumentNullException(nameof(tracingProvider));

            if (IsProviderConfigured)
            {
                return this;
            }

            if (!Providers.TryGetValue(name, out var providerOptions))
            {
                return this;
            }

            tracingProvider.RegisterProvider(providerOptions.Provider);
            IsProviderConfigured = true;

            return this;
        }
    }
}