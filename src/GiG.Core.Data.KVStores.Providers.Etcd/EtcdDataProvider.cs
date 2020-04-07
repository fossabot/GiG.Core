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
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly EtcdProviderOptions _etcdProviderOptions;
        private readonly EtcdClient _etcdClient;

        private byte _retryBackOffAmount;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{EtcdDataProvider}"/> which will be used to log events by the provider.</param>
        /// <param name="dataSerializer">The <see cref="IDataProvider{T}"/> which will be used to deserialize data from Etcd.</param>
        /// <param name="etcdProviderOptionsAccessor">The <see cref="IDataProviderOptions{T,TOptions}"/> which will be used to access options for the instance of the provider.</param>
        public EtcdDataProvider(ILogger<EtcdDataProvider<T>> logger,
            IDataSerializer<T> dataSerializer,
            IDataProviderOptions<T, EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _logger = logger;
            _dataSerializer = dataSerializer;
            _etcdProviderOptions = etcdProviderOptionsAccessor.Value;

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString, _etcdProviderOptions.Port,
                _etcdProviderOptions.Username, _etcdProviderOptions.Password, _etcdProviderOptions.CaCertificate,
                _etcdProviderOptions.ClientCertificate, _etcdProviderOptions.ClientKey,
                _etcdProviderOptions.IsPublicRootCa);
        }

        /// <inheritdoc/>
        public Task WatchAsync(Action<T> callback, params string[] keys)
        {
            var key = GetKey(keys);

            _etcdClient.Watch(key, async response =>
            {
                if (_retryBackOffAmount != 0)
                {
                    _logger.LogInformation("Watch Recovered from exception for {key}", key);

                    callback(await GetAsync());
                    _retryBackOffAmount = 0;
                }

                _logger.LogDebug("Watch Triggered for {key}", key);

                if (!response.Events.Any())
                {
                    return;
                }

                var value = response.Events[0].Kv.Value.ToStringUtf8();
                callback(_dataSerializer.GetFromString(value));
            }, null, WatchExceptionHandler);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Retrieves a model from storage using list of keys. Each key is delimited by a "/" and used to retrieve a subsection of the store.
        /// </summary>
        /// <param name="keys">The list of keys.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(params string[] keys)
        {
            var key = GetKey(keys);
           
            var value = await _etcdClient.GetValAsync(key);

            return _dataSerializer.GetFromString(value);
        }

        /// <inheritdoc/>
        public async Task WriteAsync(T model, params string[] keys)
        {
            var key = GetKey(keys);

            await _etcdClient.PutAsync(key, _dataSerializer.ConvertToString(model));
        }

        /// <inheritdoc />
        public async Task<object> LockAsync(params string[] keys)
        {
            var key = GetKey(keys);

            var lockResponse = await _etcdClient.LockAsync(key);

            return lockResponse.Key.ToStringUtf8();
        }

        /// <inheritdoc />
        public async Task UnlockAsync(object handle)
        {
            await _etcdClient.UnlockAsync(handle as string);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _etcdClient.Dispose();
        }

        private void WatchExceptionHandler(Exception ex)
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

        private string GetKey(params string[] keys)
        {
            return keys.Any()
                ? string.Concat(_etcdProviderOptions.Key, "/", string.Join("/", keys))
                : _etcdProviderOptions.Key;
        }
    }
}