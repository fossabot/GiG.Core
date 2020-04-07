using GiG.Core.Data.KVStores.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Providers.Hosting
{
    /// <summary>
    /// Data Hosted Service.
    /// </summary>
    /// <typeparam name="T">Generic to define type of hosted service.</typeparam>
    internal class DataHostedService<T> : BackgroundService
    {
        private readonly IDataRetriever<T> _dataRetriever;

        /// <summary>
        /// Data Hosted Service Constructor.
        /// </summary>
        /// <param name="dataRetriever">The <see cref="IDataRetriever{T}" /> used to fetch data from source.</param>
        public DataHostedService(IDataRetriever<T> dataRetriever)
        {
            _dataRetriever = dataRetriever;
        }

        /// <inheritdoc/>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _dataRetriever.GetAsync();
        }
    }
}