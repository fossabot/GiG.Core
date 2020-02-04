using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace GiG.Core.Benchmarks.Cache
{
    [MemoryDiagnoser]
    public class HashsetBenchmarks
    {
        private static HashSet<PasswordBlacklist> _cache;
        private static List<PasswordBlacklist> _searchList;

        [Params(10_000_000)]
        public int ReadCount { get; set; }

        private const int SearchListCount = 1_000_000;

        public HashsetBenchmarks() => _cache = BuildCache();

        [Benchmark]
        public void HashSets_LargeDataSet_ItemFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                // pick a random item from the search list to use it as a search term to search the hashset with
                var index = random.Next(1, SearchListCount);
                var searchTerm = _searchList[index];

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                _cache.Contains(searchTerm);
            }
        }

        [Benchmark]
        public void HashSets_LargeDataSet_ItemNotFound()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                // pick a random item from the search list to use it as a search term to search the hashset with
                var index = random.Next(1, SearchListCount);
                var searchTerm = _searchList[index];

                searchTerm.Value = $"{searchTerm.Value} + 3fn89r";

                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                _cache.Contains(searchTerm);
            }
        }

        private static HashSet<PasswordBlacklist> BuildCache()
        {          
            var passwordEntries = File.ReadAllLines("Cache\\Mocks\\passwordblacklist.txt");

            var passwordBlacklist = new List<PasswordBlacklist>();
            var result = new HashSet<PasswordBlacklist>();

            foreach (var entry in passwordEntries)
            {
                var passwordBlacklistEntry = new PasswordBlacklist { Value = entry };

                result.Add(passwordBlacklistEntry);
                passwordBlacklist.Add(passwordBlacklistEntry);
            }

            var random = new Random();

            // build a search list by picking one million random values 
            _searchList = new List<PasswordBlacklist>();

            for (var i = 0; i <= SearchListCount; i++)
            {
                var index = random.Next(1, SearchListCount - 10);
                var entry = passwordBlacklist[index];

                _searchList.Add(entry);
            }

            return result;
        }
    }
}