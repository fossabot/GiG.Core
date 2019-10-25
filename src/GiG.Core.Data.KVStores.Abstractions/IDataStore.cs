namespace GiG.Core.Data.KVStores.Abstractions
{
    public interface IDataStore<T>
    {
        /// <summary>
        /// Get the current model
        /// </summary>
        /// <returns></returns>
        T Get();
        
        /// <summary>
        /// Sets the model 
        /// </summary>
        /// <param name="model"></param>
        void Set(T model);
    }
}