using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Sample.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Sample
{
    public class AnimalService : IHostedService
    {
        
        private readonly IDataRetriever<IEnumerable<Animal>> _dataRetriever;

        public AnimalService(IDataRetriever<IEnumerable<Animal>> dataRetriever)
        {
            _dataRetriever = dataRetriever;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var data = _dataRetriever.Get();
            Console.WriteLine(data);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}