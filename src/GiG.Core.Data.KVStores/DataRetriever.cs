using GiG.Core.Data.KVStores.Abstractions;

namespace GiG.Core.Data.KVStores
{
    public class DataRetriever<T> : IDataRetriever<T>
    {
        private readonly IDataStore<T> _dataStore;

        public DataRetriever(IDataStore<T> dataStore)
        {
            _dataStore = dataStore;
        }
        
        public T Get()
        {
            return _dataStore.Get();
        }
    }
}