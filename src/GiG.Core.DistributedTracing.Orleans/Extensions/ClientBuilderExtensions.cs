using GiG.Core.DistributedTracing.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.DistributedTracing.Orleans.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Add Correlation Id Grain call filter.
        /// </summary>
        /// <param name="builder"><see cref="IClientBuilder"/> to add filter to.</param>
        /// <param name="serviceProvider"></param>
        /// <returns><see cref="IClientBuilder"/> to chain more methods to.</returns>
        public static IClientBuilder AddCorrelationOutgoingFilter(this IClientBuilder builder, IServiceProvider serviceProvider)
        {
            builder.ConfigureServices(services =>
                services.TryAddSingleton(serviceProvider.GetRequiredService<ICorrelationContextAccessor>()));

            return builder.AddOutgoingGrainCallFilter<CorrelationGrainCallFilter>();
        }
    }
}
