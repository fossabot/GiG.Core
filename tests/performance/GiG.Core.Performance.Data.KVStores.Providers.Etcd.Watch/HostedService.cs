using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch
{
    public class HostedService : BackgroundService
    {
        private readonly EtcdClient _etcdClient;
        private readonly ILogger<HostedService> _logger;
        private readonly string _key;

        public HostedService(EtcdClient etcdClient, ILogger<HostedService> logger, IOptions<EtcdProviderOptions> etcdProviderOptionsAccessor)
        {
            _etcdClient = etcdClient;
            _logger = logger;
            _key = etcdProviderOptionsAccessor.Value.Key;
        }
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Started Watching {key} on {date}", _key, DateTime.UtcNow);

            _etcdClient.Watch(_key, (WatchResponse response) =>
            {
                _logger.LogInformation("New Key {key} Updated on {date}", _key, DateTime.UtcNow);
            });
            
            return Task.CompletedTask;
        }
    }
}