using GiG.Core.DistributedTracing.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// Correlation Id Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Add Correlation Id Grain call filter.
        /// </summary>
        /// <param name="builder"><see cref="IClientBuilder"/> to add filter to.</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> on which to resolve context accessor.</param>
        /// <returns><see cref="IClientBuilder"/> to chain more methods to.</returns>
        public static IClientBuilder AddCorrelationOutgoingFilter([NotNull] this IClientBuilder builder, [NotNull] IServiceProvider serviceProvider)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            builder.ConfigureServices(services =>
                services.TryAddSingleton(serviceProvider.GetRequiredService<ICorrelationContextAccessor>()));

            return builder.AddOutgoingGrainCallFilter<CorrelationGrainCallFilter>();
        }
    }
}
