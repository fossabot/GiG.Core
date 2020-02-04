using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace GiG.Core.Benchmarks.Cache
{
    [MemoryDiagnoser]
    public class MasterMemoryBenchmarks
    {
        private static MemoryDatabase _largeCacheSet;
        private static MemoryDatabase _smallCacheSet;

        private static List<PasswordBlacklist> _searchList;
        
        [Params(10_000_000)]
        public int ReadCount { get; set; }

        private const int SearchListCount = 1_000_000;

        public MasterMemoryBenchmarks()
        {
            _largeCacheSet = BuildLargeCacheSet();
            _smallCacheSet = BuildSmallCacheSet();    
        }
        
        [Benchmark]
        public void MasterMemory_LargeDataSet_ItemFound()
        {            
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                // pick a random item from the search list to use it as a search term to search the cache with
                var index = random.Next(1, SearchListCount);
                var searchTerm = _searchList[index];

                _largeCacheSet.PasswordBlacklistTable.FindByValue(searchTerm.Value);                
            }
        }

        [Benchmark]
        public void MasterMemory_SmallDataSet_ItemFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1, 1000);

                _smallCacheSet.PlayerTable.FindByPlayerId(index);
            }
        }

        [Benchmark]
        public void MasterMemory_LargeDataSet_ItemNotFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                // pick a random item from the search list to use it as a search term to search the cache with
                var index = random.Next(1, SearchListCount);
                var searchTerm = _searchList[index];

                _largeCacheSet.PasswordBlacklistTable.FindByValue($"{searchTerm.Value} + 3fn89r");
            }
        }

        [Benchmark]
        public void MasterMemory_SmallDataSet_ItemNotFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1001, 9999);

                _smallCacheSet.PlayerTable.FindByPlayerId(index);
            }
        }

        private static MemoryDatabase BuildSmallCacheSet()
        {
            var builder = new DatabaseBuilder();

            var rawPlayerData = File.ReadAllText("Cache\\Mocks\\players.json");

            var players = JsonSerializer.Deserialize<Player[]>(rawPlayerData);

            builder.Append(players);

            var data = builder.Build();

            return new MemoryDatabase(data);
        }

        private static MemoryDatabase BuildLargeCacheSet()
        {
            var builder = new DatabaseBuilder();

            // the file contains 1 million password blacklist entries
            var passwordEntries = File.ReadAllLines("Cache\\Mocks\\passwordblacklist.txt");
            var passwordBlacklist = passwordEntries.Select(entry => new PasswordBlacklist {Value = entry}).ToList();

            var random = new Random();

            // build a search list by picking one million random values 
            _searchList = new List<PasswordBlacklist>();

            for (var i = 0; i <= SearchListCount; i++)
            {
                var index = random.Next(1, SearchListCount - 10);
                var entry = passwordBlacklist[index];

                _searchList.Add(entry);
            }            

            builder.Append(passwordBlacklist);

            var data = builder.Build();

            return new MemoryDatabase(data);
        }
    }
}