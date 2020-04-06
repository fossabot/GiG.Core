using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.File.Sample.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.File.Sample.Services
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

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Languages...");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(2));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var languages = _dataRetriever.GetAsync();

            _logger.LogInformation("Languages: {@languages}", languages);
        }
    }
}