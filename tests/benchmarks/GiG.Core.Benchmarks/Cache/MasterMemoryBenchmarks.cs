using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.Collections.Generic;
using System.IO;
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

        private int _searchListCount = 1_000_000;

        public MasterMemoryBenchmarks()
        {
            _largeCacheSet = BuildLargeCacheSet();
            _smallCacheSet = BuildSmallCacheSet();    
        }
        
        [Benchmark]
        public void LargeDataSet_LargeReadCount_ItemFound()
        {            
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                ///we pick a random item from the search list to use it as a search term to search the cache with
                var index = random.Next(1, _searchListCount);

                var searchTerm = _searchList[index];

                _largeCacheSet.PasswordBlacklistTable.FindByValue(searchTerm.Value);                
            }
        }

        [Benchmark]
        public void SmallDataSet_LargeReadCount_ItemFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1, 1000);

                _smallCacheSet.PlayerTable.FindByPlayerId(index);
            }
        }

        [Benchmark]
        public void LargeDataSet_LargeReadCount_ItemNotFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                ///we pick a random item from the search list to use it as a search term to search the cache with
                var index = random.Next(1, _searchListCount);

                var searchTerm = _searchList[index];

                _largeCacheSet.PasswordBlacklistTable.FindByValue(string.Format("{0} + 3fn89r", searchTerm.Value));
            }
        }

        [Benchmark]
        public void SmallDataSet_LargeReadCount_ItemNotFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1001, 9999);

                _smallCacheSet.PlayerTable.FindByPlayerId(index);
            }
        }

        private MemoryDatabase BuildSmallCacheSet()
        {
            var builder = new DatabaseBuilder();

            var rawPlayerData = File.ReadAllText("Cache\\Mocks\\players.json");

            var players = JsonSerializer.Deserialize<Player[]>(rawPlayerData);

            builder.Append(players);

            byte[] data = builder.Build();

            return new MemoryDatabase(data);
        }

        private MemoryDatabase BuildLargeCacheSet()
        {
            var builder = new DatabaseBuilder();

            //the file contains 1 million password blacklist entries
            var passwordEntries = File.ReadAllLines("Cache\\Mocks\\passwordblacklist.txt");

            var passwordBlackist = new List<PasswordBlacklist>();

            foreach (var entry in passwordEntries)
            {
                passwordBlackist.Add(new PasswordBlacklist { Value = entry });
            }

            var random = new Random();

            //we build a search list by picking one million random values 
            _searchList = new List<PasswordBlacklist>();

            for (var i = 0; i <= _searchListCount; i++)
            {
                var index = random.Next(1, _searchListCount - 10);

                var entry = passwordBlackist[index];

                _searchList.Add(entry);
            }            

            builder.Append(passwordBlackist);

            byte[] data = builder.Build();

            return new MemoryDatabase(data);
        }
    }
}