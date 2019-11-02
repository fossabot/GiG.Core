using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Hosting
{
    /// <summary>
    /// Provider Hosted Service.
    /// </summary>
    /// <typeparam name="T">Generic to define type of hosted service.</typeparam>
    public class ProviderHostedService<T> : IHostedService
    {
        private readonly IDataProvider<T> _dataProvider;

        /// <summary>
        /// ProviderHostedService Constructor.
        /// </summary>
        /// <param name="dataProvider"></param>
        public ProviderHostedService(IDataProvider<T> dataProvider)
        {
            _dataProvider = dataProvider;
        }

        /// <inheritdoc/>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _dataProvider.StartAsync();
        }

        /// <inheritdoc/>
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _dataProvider.StopAsync();
        }
    }
}