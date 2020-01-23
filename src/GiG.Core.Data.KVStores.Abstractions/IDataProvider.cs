using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// The Data Provider used to fetch data from source.
    /// </summary>
    public interface IDataProvider<T>
    {
        /// <summary>
        /// Retrieves a model from storage and stores in the registered <see cref="IDataStore{T}" />.
        /// </summary>
        Task StartAsync();

        /// <summary>
        /// Performs any required stop/shutdown procedure.
        /// </summary>
        Task StopAsync();

        /// <summary>
        /// Retrieves a model from storage using a key.
        /// </summary>
        /// <param name="keys">The key.</param>
        /// <returns></returns>
        Task<T> GetAsync(params string[] keys);
    }
}