using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Data.KVStores.Extensions
{
    public static class KVStoreBuilderExtensions
    {
        public static IKVStoreBuilder<T> AddMemoryDataStore<T>(this IKVStoreBuilder<T> builder)
        {
            builder.Services.TryAddSingleton<IDataStore<T>, MemoryDataStore<T>>();

            return builder;
        }
    }
}