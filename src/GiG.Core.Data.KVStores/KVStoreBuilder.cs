using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class KVStoreBuilder<T> : IKVStoreBuilder<T>
    {
        /// <inheritdoc />
        public KVStoreBuilder(IServiceCollection services)
        {
            Services = services;
        }
        
        /// <inheritdoc />
        public IServiceCollection Services { get;}
    }
}