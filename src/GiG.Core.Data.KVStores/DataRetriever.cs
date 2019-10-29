using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataRetriever<T> : IDataRetriever<T>
    {
        private readonly IDataStore<T> _dataStore;

        /// <inheritdoc />
        public DataRetriever(IDataStore<T> dataStore)
        {
            _dataStore = dataStore;
        }
        
        /// <inheritdoc />
        public T Get()
        {
            return _dataStore.Get();
        }
    }
}