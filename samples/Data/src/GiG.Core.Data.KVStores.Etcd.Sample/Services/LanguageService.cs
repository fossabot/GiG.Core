using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Etcd.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Etcd.Sample.Services
{
    public class LanguageService : IHostedService
    {
        private readonly IDataRetriever<IEnumerable<Language>> _dataRetriever;
        private readonly ILogger<LanguageService> _logger;
        private Timer _timer;

        public LanguageService(IDataRetriever<IEnumerable<Language>> dataRetriever, ILogger<LanguageService> logger)
        {
            _dataRetriever = dataRetriever;
            _logger = logger;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Languages...");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var data = _dataRetriever.Get();

            _logger.LogInformation("Languages: {@languages}", data);
        }
    }
}