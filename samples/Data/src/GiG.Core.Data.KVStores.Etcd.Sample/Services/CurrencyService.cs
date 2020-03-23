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
    public class CurrencyService : BackgroundService
    {
        private readonly IDataRetriever<IEnumerable<Currency>> _dataRetriever;
        private readonly IDataWriter<IEnumerable<Currency>> _dataWriter;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(
            IDataRetriever<IEnumerable<Currency>> dataRetriever, 
            IDataWriter<IEnumerable<Currency>> dataWriter, 
            ILogger<CurrencyService> logger)
        {
            _dataRetriever = dataRetriever;
            _dataWriter = dataWriter;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Writing Currencies...");

            var currencies = new []
            {
                new Currency { Name = "Euro" },
                new Currency { Name = "Dollar"}
            }.AsEnumerable();

            await _dataWriter.WriteAsync(currencies);

            _logger.LogInformation("Reading Currencies...");

            currencies = await _dataRetriever.GetAsync();

            _logger.LogInformation("Currencies: {@currencies}", currencies);
        }
    }
}