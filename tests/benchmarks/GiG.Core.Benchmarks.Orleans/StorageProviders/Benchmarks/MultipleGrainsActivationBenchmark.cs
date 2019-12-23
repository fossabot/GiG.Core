using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Benchmarks
{
    [CsvExporter]
    public class MultipleGrainsActivationBenchmark
    {
        private IClusterClient _clusterClient;

        [GlobalSetup]
        public void Setup()
        {
            _clusterClient = ClusterClientFactory.Create();
        }
        
        [Params(10, 100, 1000, 10_000)]
        public int Counter;

        [Benchmark]
        public Task Memory() => RunBenchmark<IMemoryPlayerStateWritesGrain>();

        [Benchmark]
        public Task Mongo() => RunBenchmark<IMongoPlayerStateWriterGrain>();

        [Benchmark]
        public Task Dynamo() => RunBenchmark<IDynamoPlayerStateWriterGrain>();

        [Benchmark]
        public Task Postgres() => RunBenchmark<IPostgresPlayerStateWriterGrain>();

        [Benchmark]
        public Task Redis() => RunBenchmark<IRedisPlayerStateWriterGrain>();
        
        public async Task RunBenchmark<TGrain>() where TGrain : IPlayerStateWriterGrain
        {
            for (var i = 0; i < Counter; i++)
            {
                await _clusterClient.GetGrain<TGrain>(Guid.NewGuid()).ReadPlayerDetailAsync();
            }
        }
    }
}