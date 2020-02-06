using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Etcd
{
    /// <inheritdoc />
    public class EtcdDataProvider<T> : IDataProvider<T>
    {
        private readonly ILogger<EtcdDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly IDataSerializer<T> _dataSerializer;
        private readonly EtcdProviderOptions _etcdProviderOptions;

        private readonly EtcdClient _etcdClient;

        /// <summary>
        /// Constructor
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
            _logger.LogDebug("Start Executed for {key}", _etcdProviderOptions.Key);

            var watchRequest = new WatchRequest()
            {
                CreateRequest = new WatchCreateRequest()
                {
                    Key = ByteString.CopyFromUtf8(_etcdProviderOptions.Key)
                }
            };

            _etcdClient.Watch(watchRequest, (WatchResponse response) =>
            {
                _logger.LogInformation("Watch Executed for {key}", _etcdProviderOptions.Key);

                if (response.Events.Count > 0)
                {
                    var value = response.Events[0].Kv.Value.ToStringUtf8();
                    _dataStore.Set(_dataSerializer.GetFromString(value));
                }
            });

            _dataStore.Set(await GetAsync());
        }

        /// <summary>
        /// Retrieves a model from storage using list of keys. Each key is delimited by a "/" and used to retrieve a subsection of the store.
        /// </summary>
        /// <param name="keys">The list of keys.</param>
        /// <returns></returns>
        public async Task<T> GetAsync(params string[] keys)
        {
            async Task<T> GetValueAsync(string key)
            {
                _logger.LogDebug("Returning {key}", key);

                var value = await _etcdClient.GetValAsync(key);

                return string.IsNullOrWhiteSpace(value) ? default : _dataSerializer.GetFromString(value);
            }

            if (keys.Any())
            {
                var nestedKey = string.Concat(_etcdProviderOptions.Key, "/", string.Join("/", keys));

                var value = await GetValueAsync(nestedKey);

                if (value != null)
                {
                    return value;
                }
            }

            return await GetValueAsync(_etcdProviderOptions.Key);
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            _logger.LogInformation("Stop Executed for {key}", _etcdProviderOptions.Key);

            _etcdClient.Dispose();

            return Task.CompletedTask;
        }
    }
}