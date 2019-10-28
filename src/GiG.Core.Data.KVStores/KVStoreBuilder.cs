using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Data.KVStores
{
    public class KVStoreBuilder<T> : IKVStoreBuilder<T>
    {
        public KVStoreBuilder(IServiceCollection services)
        {
            Services = services;
        }
        public IServiceCollection Services { get;}
    }
}