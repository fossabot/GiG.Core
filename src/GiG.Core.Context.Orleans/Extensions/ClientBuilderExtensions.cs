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
    /// The <see cref="IClientBuilder" /> Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Adds Request Context Grain call filter.
        /// </summary>
        /// <param name="builder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/>.</param>
        /// <returns>The <see cref="IClientBuilder"/>.</returns>
        public static IClientBuilder AddRequestContextOutgoingFilter([NotNull] this IClientBuilder builder, [NotNull] IServiceProvider serviceProvider)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            builder.ConfigureServices(services => services.TryAddSingleton(serviceProvider.GetRequiredService<IRequestContextAccessor>()));

            return builder.AddOutgoingGrainCallFilter<RequestContextGrainCallFilter>();
        }
    }
}
