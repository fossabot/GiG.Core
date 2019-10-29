namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// DataStore.
    /// </summary>
    /// <typeparam name="T">Generic to define type of data store.</typeparam>
    public interface IDataStore<T>
    {
        /// <summary>
        /// Get the current model.
        /// </summary>
        /// <returns></returns>
        T Get();
        
        /// <summary>
        /// Sets the model .
        /// </summary>
        /// <param name="model"></param>
        void Set(T model);
    }
}