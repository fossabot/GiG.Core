﻿using BenchmarkDotNet.Attributes;
using Bogus;
using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders
{
    public class StorageProvidersBenchmark
    {
        private IMemoryPlayerStateWritesGrain _memoryPlayerStateWritesGrain;
        private IMongoPlayerStateWriterGrain _mongoPlayerStateWriterGrain;
        
        [GlobalSetup]
        public void Setup()
        {
            var clusterClient = ClusterClientFactory.Create();
            
            _memoryPlayerStateWritesGrain = clusterClient.GetGrain<IMemoryPlayerStateWritesGrain>(Guid.NewGuid());
            _mongoPlayerStateWriterGrain = clusterClient.GetGrain<IMongoPlayerStateWriterGrain>(Guid.NewGuid());
        }

        [Params(1, 10)]
        public int Counter;

        [Benchmark]
        public Task Memory() => WriteStateAsync(_memoryPlayerStateWritesGrain);

        [Benchmark]
        public Task MongoDb() => WriteStateAsync(_mongoPlayerStateWriterGrain);

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