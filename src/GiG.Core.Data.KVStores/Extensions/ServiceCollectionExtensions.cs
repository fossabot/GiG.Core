using GiG.Core.Data.KVStores.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Data.KVStores.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to KV Stores functionality.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <param name="configureKVStore">A delegate that is used to configure a <see cref="IKVStoreBuilder{T}"/>.</param>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddKVStores<T>([NotNull] this IServiceCollection services, [NotNull] Action<IKVStoreBuilder<T>> configureKVStore)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configureKVStore == null) throw new ArgumentNullException(nameof(configureKVStore));
            
            services.TryAddSingleton(typeof(IDataRetriever<>), typeof(DataRetriever<>));
            services.TryAddSingleton(typeof(IDataWriter<>), typeof(DataWriter<>));
            
            var builder = new KVStoreBuilder<T>(services);
            configureKVStore(builder);

            // Add Default Services if not set by user
            builder.AddMemoryDataStore();
            builder.AddJson();

            return services;
        }
    }
}