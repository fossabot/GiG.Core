using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Writes data to the registered <see cref="IDataProvider{T}"/>.
    /// </summary>
    /// <typeparam name="T">Generic to define type of retriever.</typeparam>
    public interface IDataWriter<T>
    {
        /// <summary>
        /// Writes a model to a data provider using a key.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="keys">The key.</param>
        /// <returns></returns>
        Task WriteAsync(T value, params string[] keys);
    }
}