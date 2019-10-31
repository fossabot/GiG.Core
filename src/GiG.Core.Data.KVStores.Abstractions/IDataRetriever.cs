namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Data Retriever. Reads data from the registered <see cref="IDataStore{T}"/>.
    /// </summary>
    /// <typeparam name="T">Generic to define type of retriever.</typeparam>
    public interface IDataRetriever<T>
    {
        /// <summary>
        /// Gets a model from a data store.
        /// </summary>
        /// <returns></returns>
        T Get();
    }
}