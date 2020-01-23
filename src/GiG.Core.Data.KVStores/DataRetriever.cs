using GiG.Core.Data.KVStores.Abstractions;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataRetriever<T> : IDataRetriever<T>
    {
        private readonly IDataStore<T> _dataStore;
        private readonly IDataProvider<T> _dataProvider;

        /// <inheritdoc />
        public DataRetriever(IDataStore<T> dataStore, IDataProvider<T> dataProvider)
        {
            _dataStore = dataStore;
            _dataProvider = dataProvider;
        }
        
        /// <inheritdoc />
        public T Get()
        {
            return _dataStore.Get();
        }

        /// <inheritdoc />
        public async Task<T> GetAsync(params string[] keys)
        {
            var model = await _dataProvider.GetAsync(keys);

            if (model == null)
            {
                model = _dataStore.Get();
            }

            return model;
        }
    }
}