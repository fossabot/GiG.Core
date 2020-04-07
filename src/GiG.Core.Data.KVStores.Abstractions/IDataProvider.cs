using System;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// The Data Provider used to fetch data from source.
    /// </summary>
    public interface IDataProvider<T> : IDisposable
    {
        /// <summary>
        /// Retrieves a model from storage and stores in the registered <see cref="IDataStore{T}" />.
        /// </summary>
        Task WatchAsync(Action<T> callback, params string[] keys);


        /// <summary>
        /// Retrieves a model from storage using a key.
        /// </summary>
        /// <param name="keys">The key.</param>
        /// <returns></returns>
        Task<T> GetAsync(params string[] keys);

        /// <summary>
        /// Saves a model to storage using a key.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="keys">The key.</param>
        /// <returns></returns>
        Task WriteAsync(T model, params string[] keys);

        /// <summary>
        /// Lock mechanism to Save model without concurrency issues.
        /// </summary>
        /// <param name="keys">The key.</param>
        /// <returns>A lock handle.</returns>
        Task<object> LockAsync(params string[] keys);

        /// <summary>
        /// Unlock by using the handle provided by the LockAsync method.
        /// </summary>
        /// <param name="handle">The lock handle.</param>
        /// <returns></returns>
        Task UnlockAsync(object handle);
    }
}