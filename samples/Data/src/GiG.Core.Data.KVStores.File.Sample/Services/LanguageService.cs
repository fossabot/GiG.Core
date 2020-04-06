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
    public class LanguageService : IHostedService
    {
        private readonly IDataRetriever<IEnumerable<Language>> _dataRetriever;
        private readonly ILogger<LanguageService> _logger;

        public LanguageService(IDataRetriever<IEnumerable<Language>> dataRetriever, ILogger<LanguageService> logger)
        {
            _dataRetriever = dataRetriever;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving Languages...");
            
            var data = await _dataRetriever.GetAsync();
            _logger.LogInformation(string.Join(", ", data.Select(x=>$"Name: {x.Name}, Alpha2Code: {x.Alpha2Code}").ToArray()));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}