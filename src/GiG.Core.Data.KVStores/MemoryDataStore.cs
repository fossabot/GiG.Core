using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class MemoryDataStore<T> : IDataStore<T>
    {
        private T Data { get; set; }
       
        /// <inheritdoc />
        public T Get()
        {
            return Data;
        }
        
        /// <inheritdoc />
        public void Set(T model)
        {
            Data = model;
        }
    }
}