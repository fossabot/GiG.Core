using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

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


        /// <inheritdoc />
        public bool IsProviderRegistered { get; private set; }

        /// <inheritdoc />
        public void RegisterDataProvider<TImplementation>() where TImplementation : class, IDataProvider<T>
        {
            if (IsProviderRegistered)
            {
                throw new ApplicationException($"Data Provider for {typeof(T).FullName} has already been registered.");
            }
            
            IsProviderRegistered = true;
            
            Services.TryAddSingleton<IDataProvider<T>, TImplementation>();
        }
    }
}