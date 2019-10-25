namespace GiG.Core.Data.KVStores.Abstractions
{
    public interface IDataRetriever<T>
    {
        /// <summary>
        /// Gets a model from a data store.
        /// </summary>
        /// <returns></returns>
        T Get();
    }
}