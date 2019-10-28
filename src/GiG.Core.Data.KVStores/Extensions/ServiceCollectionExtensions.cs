using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Data.KVStores.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IKVStoreBuilder<T> AddDataProvider<T>(this IServiceCollection services)
        {
            services.TryAddSingleton(typeof(IDataStore<>), typeof(MemoryDataStore<>));
            services.TryAddSingleton(typeof(IDataRetriever<>), typeof(DataRetriever<>));
            services.AddHostedService<ProviderHostedService<T>>();
            
            return new KVStoreBuilder<T>(services);
        }
    }
}