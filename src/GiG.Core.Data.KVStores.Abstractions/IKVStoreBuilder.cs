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
        /// The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.
        /// </summary>
        IServiceCollection Services { get; }
        
        /// <summary>
        /// If set to true a Provider has already been Registered.
        /// </summary>
        bool IsProviderRegistered { get; }

        /// <summary>
        /// Registers Data Provider.
        /// </summary>
        void RegisterDataProvider<TImplementation>() where TImplementation : class, IDataProvider<T>;
    }
}