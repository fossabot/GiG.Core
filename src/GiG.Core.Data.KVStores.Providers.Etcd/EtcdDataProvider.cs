using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
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

        /// <inheritdoc />
        public EtcdDataProvider(ILogger<EtcdDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataSerializer<T> dataSerializer,
            IDataProviderOptions<T, EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _dataSerializer = dataSerializer;
            _etcdProviderOptions = etcdProviderOptionsAccessor.Value;

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString);
        }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            _logger.LogInformation("Start Executed for {key}", _etcdProviderOptions.Key);

            var value = await _etcdClient.GetValAsync(_etcdProviderOptions.Key);

            if (string.IsNullOrWhiteSpace(value))
            {
                _dataStore.Set(default);
            }
            else
            {
                _dataStore.Set(_dataSerializer.GetFromString(value));
            }

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
                    value = response.Events[0].Kv.Value.ToStringUtf8();
                    _dataStore.Set(_dataSerializer.GetFromString(value));
                }
            });
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