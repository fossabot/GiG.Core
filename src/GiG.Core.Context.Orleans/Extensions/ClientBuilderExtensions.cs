using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orleans;
using System;

namespace GiG.Core.Context.Orleans.Extensions
{
    /// <summary>
    /// Reqeust Context Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Add Request Context Grain call filter.
        /// </summary>
        /// <param name="builder"><see cref="IClientBuilder"/> to add filter to.</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> on which to resolve request context accessor.</param>
        /// <returns><see cref="IClientBuilder"/> to chain more methods to.</returns>
        public static IClientBuilder AddRequestContextOutgoingFilter([NotNull] this IClientBuilder builder, [NotNull] IServiceProvider serviceProvider)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            builder.ConfigureServices(services =>
                services.TryAddSingleton(serviceProvider.GetRequiredService<IRequestContextAccessor>()));

            return builder.AddOutgoingGrainCallFilter<RequestContextGrainCallFilter>();
        }
    }
}
