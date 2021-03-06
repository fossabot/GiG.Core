using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.Serializers;
using GiG.Core.Data.Serializers.Abstractions;
using GiG.Core.Data.Serializers.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Data.KVStores.Extensions
{
    /// <summary>
    /// The <see cref="IKVStoreBuilder{T}" /> Extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// Adds required services to Memory Data Store functionality.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        public static IKVStoreBuilder<T> AddMemoryDataStore<T>([NotNull] this IKVStoreBuilder<T> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            builder.Services.TryAddSingleton<IDataStore<T>, MemoryDataStore<T>>();

            return builder;
        }

        /// <summary>
        /// Adds required services for Json data serialization.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>  
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        public static IKVStoreBuilder<T> AddJson<T>([NotNull] this IKVStoreBuilder<T> builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.Services.AddSystemTextJsonDataSerializer<T>();

            return builder;
        }
    }
}