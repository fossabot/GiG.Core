using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Etcd.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Etcd.Sample.Services
{
    public class CurrencyService : IHostedService
    {
        private readonly IDataRetriever<IEnumerable<Currency>> _dataRetriever;
        private readonly IDataProvider<IEnumerable<Currency>> _dataProvider;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(
            IDataRetriever<IEnumerable<Currency>> dataRetriever, 
            IDataProvider<IEnumerable<Currency>> dataProvider, 
            ILogger<CurrencyService> logger)
        {
            _dataRetriever = dataRetriever;
            _dataProvider = dataProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing Currencies...");

            var currencies = new Currency[]
            {
                new Currency { Name = "Euro" },
                new Currency { Name = "Dollar"}
            }.AsEnumerable();

            await _dataProvider.WriteAsync(currencies);

            _logger.LogInformation("Reading Currencies...");

            currencies = await _dataRetriever.GetAsync();

            _logger.LogInformation("Currencies: {@currencies}", currencies);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}