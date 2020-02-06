using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.File.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.File.Sample.Services
{
    public class CurrencyService : IHostedService
    {
        private readonly IDataRetriever<IEnumerable<Currency>> _dataRetriever;
        private readonly ILogger<CurrencyService> _logger;

        public CurrencyService(IDataRetriever<IEnumerable<Currency>> dataRetriever, ILogger<CurrencyService> logger)
        {
            _dataRetriever = dataRetriever;
            _logger = logger;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Currencies...");
           
            var data = _dataRetriever.Get();
            _logger.LogInformation(string.Join(", ", data.Select(x => $"Name: {x.Name}").ToArray()));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}