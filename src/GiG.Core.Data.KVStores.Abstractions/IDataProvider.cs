using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Data Provider. Used to fetch data from source.
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
    }
}