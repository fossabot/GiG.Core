using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Metadata = Grpc.Core.Metadata;

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
        private readonly bool _isBasicAuthRequired;

        private byte _retryBackOffAmount;
        private Metadata _metadata;

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

            _isBasicAuthRequired = !string.IsNullOrEmpty(_etcdProviderOptions.Username) &&
                                   !string.IsNullOrEmpty(_etcdProviderOptions.Password);
        }

        /// <inheritdoc/>
        public async Task WatchAsync(Action<T> callback, params string[] keys)
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
            }, await GetMetadataAsync(), WatchExceptionHandler);
        }

        /// <summary>
        /// Retrieves a model from storage using list of keys. Each key is delimited by a "/" and used to retrieve a subsection of the store.
        /// </summary>
        /// <param name="keys">The list of keys.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(params string[] keys)
        {
            var key = GetKey(keys);

            var value = await _etcdClient.GetValAsync(key, await GetMetadataAsync());

            return _dataSerializer.GetFromString(value);
        }

        /// <inheritdoc/>
        public async Task WriteAsync(T model, params string[] keys)
        {
            var key = GetKey(keys);

            await _etcdClient.PutAsync(key, _dataSerializer.ConvertToString(model), await GetMetadataAsync());
        }

        /// <inheritdoc />
        public async Task<object> LockAsync(params string[] keys)
        {
            var key = $"locks/{GetKey(keys)}";

            var lockResponse = await _etcdClient.LockAsync(key, await GetMetadataAsync());

            return lockResponse.Key.ToStringUtf8();
        }

        /// <inheritdoc />
        public async Task UnlockAsync(object handle)
        {
            if (handle is string handleString)
            {
                await _etcdClient.UnlockAsync(handleString, await GetMetadataAsync());
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _etcdClient.Dispose();
        }
        
        private async Task<Metadata> GetMetadataAsync()
        {
            if (!_isBasicAuthRequired)
            {
                return null;
            }

            if (_metadata != null)
            {
                return _metadata;
            }

            // Temporary fix until bug is fixed - https://github.com/shubhamranjan/dotnet-etcd/issues/48
            var authenticateResponse = await _etcdClient.AuthenticateAsync(new AuthenticateRequest
                {Name = _etcdProviderOptions.Username, Password = _etcdProviderOptions.Password});
            _metadata = new Metadata {{"Authorization", authenticateResponse.Token}};

            return _metadata;
        }

        private void WatchExceptionHandler(Exception ex)
        {
            // Exponential back-off plus some jitter
            var jitter = new Random();
            var delay = TimeSpan.FromSeconds(Math.Pow(2, _retryBackOffAmount)) +
                        TimeSpan.FromMilliseconds(jitter.Next(0, 250));

            _logger.LogError(ex, "Watch failed due to an exception {message} for {key}. Trying again in {delay}",
                ex.Message, _etcdProviderOptions.Key, delay);

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