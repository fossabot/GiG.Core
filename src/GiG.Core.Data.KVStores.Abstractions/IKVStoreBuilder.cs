using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Data.KVStores.Abstractions
{
    public interface IKVStoreBuilder<T>
    {
        IServiceCollection Services { get; }
    }
}