using BenchmarkDotNet.Attributes;
using Bogus;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Benchmarks
{
    [CsvExporter]
    public class MultipleGrainsReadsBenchmark
    {
        private List<Guid> _grainIds;
        
        [GlobalSetup]
        public void Setup()
        {
            CreateGrains().GetAwaiter().GetResult();
        }
        
        [Params(10, 100, 1000, 10_000)]
        public int Counter;

        [Benchmark]
        public Task Memory() => ActivateGrains<IMemoryPlayerStateWritesGrain>();

        [Benchmark]
        public Task Mongo() => ActivateGrains<IMongoPlayerStateWriterGrain>();

        [Benchmark]
        public Task Dynamo() => ActivateGrains<IDynamoPlayerStateWriterGrain>();

        [Benchmark]
        public Task  Postgres() => ActivateGrains<IPostgresPlayerStateWriterGrain>();

        [Benchmark]
        public Task Redis() => ActivateGrains<IRedisPlayerStateWriterGrain>();

        private async Task ActivateGrains<TGrain>() where TGrain : IPlayerStateWriterGrain
        {
            using var clusterClient = ClusterClientFactory.Create();
            for (var i = 0; i < Counter; i++)
            {
                await clusterClient.GetGrain<TGrain>(_grainIds[i]).ReadPlayerDetailAsync();
            }
        }
        
        private async Task CreateGrains()
        {
            using var clusterClient = ClusterClientFactory.Create();
            _grainIds = new List<Guid>();
            
            var randomizer = new Faker().Random;
            
            for (var i = 0; i < Counter; i++)
            {
                var grainId = Guid.NewGuid();
                var firstName = randomizer.String2(10);
                var lastName = randomizer.String2(12);

                await PersistGrain<IMemoryPlayerStateWritesGrain>(clusterClient, grainId, firstName, lastName);
                await PersistGrain<IMongoPlayerStateWriterGrain>(clusterClient, grainId, firstName, lastName);
                await PersistGrain<IDynamoPlayerStateWriterGrain>(clusterClient, grainId, firstName, lastName);
                await PersistGrain<IPostgresPlayerStateWriterGrain>(clusterClient, grainId, firstName, lastName);
                await PersistGrain<IRedisPlayerStateWriterGrain>(clusterClient, grainId, firstName, lastName);
                
                _grainIds.Add(grainId);
            }
        }

        private async Task PersistGrain<TGrain>(IClusterClient clusterClient, Guid grainId, string firstName, string lastName) where TGrain : IPlayerStateWriterGrain
        {
            await clusterClient.GetGrain<TGrain>(grainId).WritePlayerDetailAsync(firstName, lastName);
        }
    }
}