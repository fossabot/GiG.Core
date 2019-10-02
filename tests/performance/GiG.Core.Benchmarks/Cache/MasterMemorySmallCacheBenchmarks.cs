using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.IO;
using System.Text.Json;

namespace GiG.Core.Benchmarks.Cache
{
    [MemoryDiagnoser]
    public class MasterMemorySmallCacheBenchmarks
    {
        private static MemoryDatabase _cache;
        
        [Params(10_000_000)]
        public int ReadCount { get; set; }

        public MasterMemorySmallCacheBenchmarks()
        {
            _cache = BuildSmallCacheStore();
        }
        
        [Benchmark]
        public void ReadOperations_SmallDataSet_LargeReadCount()
        {            
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1, 1000);

                _cache.PlayerTable.FindByPlayerId(index);                
            }
        }

        private MemoryDatabase BuildSmallCacheStore()
        {
            var builder = new DatabaseBuilder();

            var rawPlayerData = File.ReadAllText("Cache\\Mocks\\players.json");

            var players = JsonSerializer.Deserialize<Player[]>(rawPlayerData);

            builder.Append(players);

            byte[] data = builder.Build();

            return new MemoryDatabase(data);
        }
    }
}
