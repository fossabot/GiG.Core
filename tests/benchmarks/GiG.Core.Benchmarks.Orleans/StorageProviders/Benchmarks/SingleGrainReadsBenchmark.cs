using BenchmarkDotNet.Attributes;
using Bogus;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Benchmarks
{
    [CsvExporter]
    public class SingleGrainReadsBenchmark
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

            WritePlayerDetailsAsync().GetAwaiter().GetResult();
        }
        
        [Params(10, 100, 1000, 10_000)]
        public int Counter;

        [Benchmark]
        public Task Memory() => ReadPlayerDetailAsync(_memoryPlayerStateWritesGrain);

        [Benchmark]
        public Task MongoDb() => ReadPlayerDetailAsync(_mongoPlayerStateWriterGrain);

        [Benchmark]
        public Task DynamoDb() => ReadPlayerDetailAsync(_dynamoPlayerStateWriterGrain);

        [Benchmark]
        public Task Postgres() => ReadPlayerDetailAsync(_postgresPlayerStateWriterGrain);

        [Benchmark]
        public Task Redis() => ReadPlayerDetailAsync(_redisPlayerStateWriterGrain);
        
        private async Task ReadPlayerDetailAsync(IPlayerStateWriterGrain grain)
        {
            for (var i = 0; i < Counter; i++)
            {
                await grain.ReadPlayerDetailAsync();    
            }
        }
        
        private async Task WritePlayerDetailsAsync()
        {
            var ranomizer = new Faker().Random;

            var firstName = ranomizer.String2(10);
            var lastName = ranomizer.String2(12);

            await _memoryPlayerStateWritesGrain.WritePlayerDetailAsync(firstName, lastName);
            await _mongoPlayerStateWriterGrain.WritePlayerDetailAsync(firstName, lastName);
            await _dynamoPlayerStateWriterGrain.WritePlayerDetailAsync(firstName, lastName);
            await _postgresPlayerStateWriterGrain.WritePlayerDetailAsync(firstName, lastName);
            await _redisPlayerStateWriterGrain.WritePlayerDetailAsync(firstName, lastName);
        }
        
    }
}