using GiG.Core.Data.KVStores.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Data.KVStores.Providers.Hosting
{
    /// <summary>
    /// The <see cref="IKVStoreBuilder{T}" /> extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// Adds Eager loading functionality for Root Key.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        public static IKVStoreBuilder<T> WithEagerLoading<T>([NotNull] this IKVStoreBuilder<T> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddHostedService<ProviderHostedService<T>>();

            return builder;
        }
    }
}