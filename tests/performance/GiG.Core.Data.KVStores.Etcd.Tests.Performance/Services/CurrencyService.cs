using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Etcd.Tests.Performance.Models;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Etcd.Tests.Performance.Services
{
    public class CurrencyService : IHostedService
    {
        private readonly IDataWriter<IEnumerable<Currency>> _dataWriter;

        public CurrencyService(IDataWriter<IEnumerable<Currency>> dataWriter)
        {
            _dataWriter = dataWriter;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var currencies = new []
            {
                new Currency { Name = "EUR" },
                new Currency { Name = "USD" },
                new Currency { Name = "GBP" } 
            };

            await _dataWriter.WriteAsync(currencies);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}