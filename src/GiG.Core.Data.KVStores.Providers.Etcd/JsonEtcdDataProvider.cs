using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace GiG.Core.Data.KVStores.Providers.Etcd
{
    /// <inheritdoc />
    public class JsonEtcdDataProvider<T> : EtcdDataProvider<T>
    {
        /// <inheritdoc />
        public JsonEtcdDataProvider(
            ILogger<JsonEtcdDataProvider<T>> logger,
            IDataStore<T> dataStore,
            IDataProviderOptions<T, EtcdProviderOptions> etcdProviderOptionsAccessor) :
            base(logger, dataStore,  etcdProviderOptionsAccessor)
        {

        }

        /// <inheritdoc />
        protected override T GetFromString(string value)
        {
            return JsonSerializer.Deserialize<T>(value);
        }
    }
}