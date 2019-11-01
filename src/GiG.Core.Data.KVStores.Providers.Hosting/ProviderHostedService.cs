using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Hosting
{
    /// <summary>
    /// ProviderHostedService.
    /// </summary>
    /// <typeparam name="T">Generic to define type of hosted service.</typeparam>
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