using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Data.KVStores.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to Data Providers functionality.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>        
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>

        public static IKVStoreBuilder<T> AddDataProvider<T>([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.TryAddSingleton(typeof(IDataStore<>), typeof(MemoryDataStore<>));
            services.TryAddSingleton(typeof(IDataRetriever<>), typeof(DataRetriever<>));
            services.AddHostedService<ProviderHostedService<T>>();
            
            return new KVStoreBuilder<T>(services);
        }
    }
}