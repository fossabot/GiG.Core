namespace GiG.Core.Data.KVStores.Abstractions
{
    /// <summary>
    /// Provides functionality to store and retrieve data.
    /// </summary>
    /// <typeparam name="T">Generic to define type of data store.</typeparam>
    public interface IDataStore<T>
    {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        T Get(params string[] keys);

        /// <summary>
        /// Sets the model.
        /// </summary>
        /// <param name="value">The model value.</param>
        /// <param name="keys">The keys.</param>
        void Set(T value, params string[] keys);
    }
}