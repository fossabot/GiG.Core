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
    public abstract class EtcdDataProvider<T> : IDataProvider<T>
    {
        private readonly ILogger<EtcdDataProvider<T>> _logger;
        private readonly IDataStore<T> _dataStore;
        private readonly EtcdProviderOptions _etcdProviderOptions;

        private readonly EtcdClient _etcdClient;

        /// <inheritdoc />
        protected EtcdDataProvider(ILogger<EtcdDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataProviderOptions<T, EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _logger = logger;
            _dataStore = dataStore;
            _etcdProviderOptions = etcdProviderOptionsAccessor.Value;

            _etcdClient = new EtcdClient(_etcdProviderOptions.ConnectionString);
        }

        /// <inheritdoc/>
        public async Task StartAsync()
        {
            var value = await _etcdClient.GetValAsync(_etcdProviderOptions.Key);

            if (string.IsNullOrWhiteSpace(value))
            {
                _dataStore.Set(default);
            }
            else
            {
                _dataStore.Set(GetFromString(value));
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
                if (response.Events.Count > 0)
                {
                    value = response.Events[0].Kv.Value.ToStringUtf8();
                    _dataStore.Set(GetFromString(value));
                }
            });
        }

        /// <inheritdoc/>
        public Task StopAsync()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Get Model from String.
        /// </summary>
        /// <param name="value">The <see cref="string"/>.</param>
        /// <returns>Generic to define type of model.</returns>
        protected abstract T GetFromString(string value);
    }
}