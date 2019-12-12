using BenchmarkDotNet.Attributes;
using Bogus;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Benchmarks
{
    [CsvExporter]
    public class SingleGrainWritesBenchmark
    {
        private IMemoryPlayerStateWritesGrain _memoryPlayerStateWritesGrain;
        private IMongoPlayerStateWriterGrain _mongoPlayerStateWriterGrain;
        private IDynamoPlayerStateWriterGrain _dynamoPlayerStateWriterGrain;
        private IPostgresPlayerStateWriterGrain _postgresPlayerStateWriterGrain;
        private IRedisPlayerStateWriterGrain _redisPlayerStateWriterGrain;
        
        [GlobalSetup]
        public void Setup()
        {
            var clusterClient = ClusterClientFactory.Create();
            
            _memoryPlayerStateWritesGrain = clusterClient.GetGrain<IMemoryPlayerStateWritesGrain>(Guid.NewGuid());
            _mongoPlayerStateWriterGrain = clusterClient.GetGrain<IMongoPlayerStateWriterGrain>(Guid.NewGuid());
            _dynamoPlayerStateWriterGrain = clusterClient.GetGrain<IDynamoPlayerStateWriterGrain>(Guid.NewGuid());
            _postgresPlayerStateWriterGrain = clusterClient.GetGrain<IPostgresPlayerStateWriterGrain>(Guid.NewGuid());
            _redisPlayerStateWriterGrain = clusterClient.GetGrain<IRedisPlayerStateWriterGrain>(Guid.NewGuid());
        }

        [Params(10, 100, 1000, 10_000)]
        public int Counter;

        [Benchmark]
        public Task Memory() => WriteStateAsync(_memoryPlayerStateWritesGrain);

        [Benchmark]
        public Task MongoDb() => WriteStateAsync(_mongoPlayerStateWriterGrain);

        [Benchmark]
        public Task DynamoDb() => WriteStateAsync(_dynamoPlayerStateWriterGrain);

        [Benchmark]
        public Task Postgres() => WriteStateAsync(_postgresPlayerStateWriterGrain);

        [Benchmark]
        public Task Redis() => WriteStateAsync(_redisPlayerStateWriterGrain);

        private async Task WriteStateAsync(IPlayerStateWriterGrain grain)
        {
            var ranomizer = new Faker().Random;
            
            for (var i = 0; i < Counter; i++)
            {
                await grain.WritePlayerDetailAsync(ranomizer.String2(10), ranomizer.String2(12));
            }
        }
    }
}