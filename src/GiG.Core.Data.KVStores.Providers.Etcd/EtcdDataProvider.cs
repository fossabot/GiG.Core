using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Etcd
{
    /// <inheritdoc />
    public class EtcdDataProvider<T> : IDataProvider<T>
    {
        private const byte MaxBackOffAmount = 5;
        
        private readonly ILogger<EtcdDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly EtcdProviderOptions _etcdProviderOptions;
        private readonly EtcdClient _etcdClient;

        private byte _retryBackOffAmount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{EtcdDataProvider}"/> which will be used to log events by the provider.</param>
        /// <param name="dataStore">The <see cref="IDataStore{T}" /> which will be used to store the data retrieved by the provider.</param>
        /// <param name="dataSerializer">The <see cref="IDataProvider{T}"/> which will be used to deserialize data from Etcd.</param>
        /// <param name="etcdProviderOptionsAccessor">The <see cref="IDataProviderOptions{T,TOptions}"/> which will be used to access options for the instance of the provider.</param>
        public EtcdDataProvider(ILogger<EtcdDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataSerializer<T> dataSerializer,
            IDataProviderOptions<T, EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _dataSerializer = dataSerializer;
            _etcdProviderOptions = etcdProviderOptionsAccessor.Value;

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString, _etcdProviderOptions.Port,
                _etcdProviderOptions.Username, _etcdProviderOptions.Password, _etcdProviderOptions.CaCertificate,
                _etcdProviderOptions.ClientCertificate, _etcdProviderOptions.ClientKey,
                _etcdProviderOptions.IsPublicRootCa);
        }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            _logger.LogDebug("Started Watch for {key}", _etcdProviderOptions.Key);

            Watch(_etcdProviderOptions.Key);

            _dataStore.Set(await GetAsync());
        }

        /// <summary>
        /// Retrieves a model from storage using list of keys. Each key is delimited by a "/" and used to retrieve a subsection of the store.
        /// </summary>
        /// <param name="keys">The list of keys.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(params string[] keys)
        {
            var key = GetKey();

            _logger.LogDebug("Returning {key}", key);

            var value = await _etcdClient.GetValAsync(key);

            return SerializeValue(value);
        }

        /// <inheritdoc/>
        public async Task WriteAsync(T model, params string[] keys)
        {
            var key = GetKey();

            _logger.LogDebug("Writing {key}", key);

            var lockResponse = await _etcdClient.LockAsync(key);
            await _etcdClient.PutAsync(key, _dataSerializer.ConvertToString(model));
            await _etcdClient.UnlockAsync(lockResponse.Key.ToStringUtf8());
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            _logger.LogDebug("Stop Executed for {key}", _etcdProviderOptions.Key);

            _etcdClient.Dispose();

            return Task.CompletedTask;
        }

        private void Watch(string key)
        {
            _etcdClient.Watch(key, async response =>
            {
                if (_retryBackOffAmount != 0)
                {
                    _logger.LogInformation("Watch Recovered from exception for {key}", _etcdProviderOptions.Key);

                    _dataStore.Set(await GetAsync());
                    _retryBackOffAmount = 0;
                }

                _logger.LogDebug("Watch Triggered for {key}", _etcdProviderOptions.Key);

                if (!response.Events.Any())
                {
                    return;
                }

                var value = response.Events[0].Kv.Value.ToStringUtf8();
                _dataStore.Set(SerializeValue(value));
            }, null, WatchExceptionHandlerAsync);
        }

        private void WatchExceptionHandlerAsync(Exception ex)
        {
            // Exponential back-off plus some jitter
            var jitter = new Random();
            var delay = TimeSpan.FromSeconds(Math.Pow(2, _retryBackOffAmount)) + TimeSpan.FromMilliseconds(jitter.Next(0, 250));

            _logger.LogError(ex, "Watch failed due to an exception {message} for {key}. Trying again in {delay}", ex.Message, _etcdProviderOptions.Key, delay);

            if (_retryBackOffAmount < MaxBackOffAmount)
            {
                _retryBackOffAmount++;
            }

            Thread.Sleep(delay);
        }

        private T SerializeValue(string value)
        {
            return !string.IsNullOrWhiteSpace(value)
                ? _dataSerializer.GetFromString(value)
                : default;
        }

        private string GetKey(params string[] keys)
        {
            return keys.Any()
                ? string.Concat(_etcdProviderOptions.Key, "/", string.Join("/", keys))
                : _etcdProviderOptions.Key;
        }
    }
}