using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.Performance.Orleans.Streams.Producer
{
    class HostedService : BackgroundService
    {
        private readonly IClusterClient _clusterClient;

        public HostedService(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!_clusterClient.IsInitialized)
            {
                await Task.Delay(500);
            }

            Console.WriteLine("Initializing SMS Provider Test");
            var smsProviderGrain = _clusterClient.GetGrain<ISMSProviderProducerGrain>(new System.Guid());
            await TestProvider(smsProviderGrain);

            Console.WriteLine("Initializing Kafka Provider Test");
            var kafkaProviderGrain = _clusterClient.GetGrain<IKafkaProviderProducerGrain>(new System.Guid());
            await TestProvider(kafkaProviderGrain);
        }

        private async Task TestProvider(IProducerGrain grain, int counter = 1_00_000)
        {
            var watch = new Stopwatch();

            Console.WriteLine("Start Warmup Warmup");
            for (var i = 0; i < 1000; i++)
            {
                await grain.ProduceAsync("TestHeader", "TestBody");
            }
            Console.WriteLine("End Warmup");

            watch.Start();

            for (var i = 0; i < counter; i++)
            {
                await grain.ProduceAsync("TestHeader", "TestBody");
            }

            watch.Stop();
            Console.WriteLine((counter / (watch.ElapsedMilliseconds * 1000.00)));
        }
    }
}
