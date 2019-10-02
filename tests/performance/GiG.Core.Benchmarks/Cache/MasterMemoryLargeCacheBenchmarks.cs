using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace GiG.Core.Benchmarks.Cache
{
    [MemoryDiagnoser]
    public class MasterMemoryLargeCacheBenchmarks
    {
        private static MemoryDatabase _cache;
        private static List<PasswordBlacklist> _searchList;
        
        [Params(10_000_000)]
        public int ReadCount { get; set; }

        private int _searchListCount = 1_000_000;

        public MasterMemoryLargeCacheBenchmarks()
        {
            _cache = BuildCache();
        }
        
        [Benchmark]
        public void ReadOperations_SmallDataSet_LargeReadCount()
        {            
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                ///we pick a random item from the search list to use it as a search term to search the cache with
                var index = random.Next(1, _searchListCount);

                var searchTerm = _searchList[index];

                _cache.PasswordBlacklistTable.FindByValue(searchTerm.Value);                
            }
        }

        private MemoryDatabase BuildCache()
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
                var index = random.Next(1, _searchListCount);

                var entry = passwordBlackist[index];

                _searchList.Add(entry);
            }            

            builder.Append(passwordBlackist);

            byte[] data = builder.Build();

            return new MemoryDatabase(data);
        }
    }
}
