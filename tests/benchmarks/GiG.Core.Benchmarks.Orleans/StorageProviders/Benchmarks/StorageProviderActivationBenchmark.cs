using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans;
using System;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Benchmarks
{
    public class StorageProviderActivationBenchmark
    {
        private IClusterClient _clusterClient;

        [GlobalSetup]
        public void Setup()
        {
            _clusterClient = ClusterClientFactory.Create();
        }
        
        [Params(10)]
        public int Counter;

        [Benchmark]
        public void Memory() => RunBenchmark<IMemoryPlayerStateWritesGrain>();

        [Benchmark]
        public void Mongo() => RunBenchmark<IMongoPlayerStateWriterGrain>();

        [Benchmark]
        public void Dynamo() => RunBenchmark<IDynamoPlayerStateWriterGrain>();

        [Benchmark]
        public void Postgres() => RunBenchmark<IPostgresPlayerStateWriterGrain>();

        [Benchmark]
        public void Redis() => RunBenchmark<IRedisPlayerStateWriterGrain>();
        
        private void RunBenchmark<TGrain>() where TGrain : IPlayerStateWriterGrain
        {
            for (var i = 0; i < Counter; i++)
            {
                _clusterClient.GetGrain<TGrain>(Guid.NewGuid());
            }
        }
    }
}