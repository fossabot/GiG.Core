using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Orleans.Streams.Contracts;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.Streams
{
    public class StreamsBenchmark
    {
        private ISMSProviderProducerGrain _smsProviderGrain;
        private IKafkaProviderProducerGrain _kafkaProviderGrain;
      
        [GlobalSetup]
        public void Setup()
        {
            var clusterClient = ClusterClientFactory.Create();

            Console.WriteLine("Initializing SMS Provider Test");
            _smsProviderGrain = clusterClient.GetGrain<ISMSProviderProducerGrain>(Guid.NewGuid());

            Console.WriteLine("Initializing Kafka Provider Test");
            _kafkaProviderGrain = clusterClient.GetGrain<IKafkaProviderProducerGrain>(Guid.NewGuid());
        }

        [Params(1, 10, 100, 1_000, 10_000, 100_000)]
        public int Counter;

        [Benchmark]
        public Task SMS() => ProduceAsync(_smsProviderGrain);

        [Benchmark]
        public Task Kafka() => ProduceAsync(_kafkaProviderGrain);

        public async Task ProduceAsync(IProducerGrain grain)
        {
            for (var i = 0; i < Counter; i++)
            {
                await grain.ProduceAsync("TestHeader", "TestBody");
            }
        }
    }
}