using GiG.Core.Data.KVStores.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores
{
    /// <inheritdoc />
    public class DataRetriever<T> : IDataRetriever<T>
    {
        private readonly IDataProvider<T> _dataProvider;
        private readonly IDataStore<T> _dataStore;
        private readonly SemaphoreSlim _readLock = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Initializes a new instance of the DataRetriever class.
        /// </summary>
        /// <param name="dataProvider">The data provider used to fetch data from source.</param>
        /// <param name="dataStore">The data store used to cache the value after it is fetched from source.</param>
        public DataRetriever(IDataProvider<T> dataProvider, IDataStore<T> dataStore)
        {
            _dataProvider = dataProvider;
            _dataStore = dataStore;
        }

        /// <inheritdoc />
        public async Task<T> GetAsync(params string[] keys)
        {
            // This is being done outside of lock for optimistic locking.
            var value = _dataStore.Get(keys);
            if (value != null)
            {
                return value;
            }

            await _readLock.WaitAsync();
            try
            {
                value = _dataStore.Get(keys);
                if (value != null)
                {
                    return value;
                }

                value = await GetFromProviderAsync(keys);
                await _dataProvider.WatchAsync(data => _dataStore.Set(data, keys), keys);

                return value;
            }
            finally
            {
                _readLock.Release();
            }
        }

        private async Task<T> GetFromProviderAsync(string[] keys)
        {
            var value = await _dataProvider.GetAsync(keys);

            // Fallback to default
            if (value == null && keys.Any())
            {
                value = _dataStore.Get();
                if (value != null)
                {
                    return value;
                }
                
                return await _dataProvider.GetAsync();
            }

            _dataStore.Set(value, keys);

            return value;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _dataProvider?.Dispose();
        }
    }
}