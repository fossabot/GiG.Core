using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class KVStoreBuilder<T> : IKVStoreBuilder<T>
    {
        /// <summary>
        /// Initializes a new instance of the KVStoreBuilder class.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        public KVStoreBuilder(IServiceCollection services)
        {
            Services = services;
        }
        
        /// <inheritdoc />
        public IServiceCollection Services { get;}
    }
}