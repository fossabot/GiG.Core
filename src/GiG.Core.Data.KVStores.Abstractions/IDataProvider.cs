using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Data Provider.
    /// </summary>
    public interface IDataProvider<T>
    {
        /// <summary>
        /// Retrieves a model from storage and stores .
        /// </summary>
        Task StartAsync();

        Task StopAsync();
    }
}