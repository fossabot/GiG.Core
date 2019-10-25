using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Hosting
{
    public class ProviderHostedService<T> : IHostedService
    {
        private readonly IDataProvider<T> _dataProvider;

        public ProviderHostedService(IDataProvider<T> dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _dataProvider.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _dataProvider.StopAsync();
        }
    }
}