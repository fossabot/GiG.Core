using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Sample.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Data.KVStores.Sample
{
    public class PersonService : IHostedService
    {
        
        private readonly IDataRetriever<IEnumerable<Person>> _dataRetriever;

        public PersonService(IDataRetriever<IEnumerable<Person>> dataRetriever)
        {
            _dataRetriever = dataRetriever;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine(_dataRetriever.Get());

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}