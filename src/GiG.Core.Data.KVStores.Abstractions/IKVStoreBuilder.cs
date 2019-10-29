using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// KVStoreBuilder.
    /// </summary>
    /// <typeparam name="T">Generic to define type of builder.</typeparam>
    public interface IKVStoreBuilder<T>
    {
        /// <summary>
        ///  The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
        /// </summary>
        IServiceCollection Services { get; }
    }
}