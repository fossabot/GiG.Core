using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.WatchRange
{
    public class HostedService : BackgroundService
    {
        private readonly EtcdClient _etcdClient;
        private readonly ILogger<HostedService> _logger;
        private readonly string _keyPrefix;

        public HostedService(EtcdClient etcdClient, ILogger<HostedService> logger, IOptions<EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _etcdClient = etcdClient;
            _logger = logger;
            _keyPrefix = etcdProviderOptionsAccessor.Value.Key;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Started Watching Range {key} on {date}", _keyPrefix, DateTime.UtcNow);

            _etcdClient.WatchRange(_keyPrefix, (WatchResponse response) =>
            {
                _logger.LogInformation("New Key {key} Updated on {date}. Number of Events {eventsCount}", response.Events.FirstOrDefault()?.Kv.Key.ToStringUtf8() ??  _keyPrefix, DateTime.UtcNow, response.Events.Count);
            });

            return Task.CompletedTask;
        }
    }
}