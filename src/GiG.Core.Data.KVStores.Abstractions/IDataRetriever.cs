using System;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Reads data from the registered <see cref="IDataProvider{T}"/>.
    /// </summary>
    /// <typeparam name="T">Generic to define type of retriever.</typeparam>
    public interface IDataRetriever<T> : IDisposable
    {
        /// <summary>
        /// Gets a model from a data store using a key.
        /// </summary>
        /// <param name="keys">The key.</param>
        /// <returns></returns>
        Task<T> GetAsync(params string[] keys);
    }
}