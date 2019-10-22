using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Adds the Correlation ID Grain outgoing filter.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder AddCorrelationOutgoingFilter([NotNull] this IClientBuilder builder, [NotNull] IServiceProvider serviceProvider)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            builder.ConfigureServices(services => services.TryAddSingleton(serviceProvider.GetRequiredService<ICorrelationContextAccessor>()));

            return builder.AddOutgoingGrainCallFilter<CorrelationGrainCallFilter>();
        }
    }
}