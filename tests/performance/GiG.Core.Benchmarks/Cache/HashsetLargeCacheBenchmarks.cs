using BenchmarkDotNet.Attributes;
using GiG.Core.Benchmarks.Cache.Mocks;
using System;
using System.Collections.Generic;
using System.IO;

namespace GiG.Core.Benchmarks.Cache
{
    [MemoryDiagnoser]
    public class HashsetLargeCacheBenchmarks
    {
        private static HashSet<PasswordBlacklist> _cache;
        private static List<PasswordBlacklist> _searchList;

        [Params(10_000_000)]
        public int ReadCount { get; set; }

        private int _searchListCount = 1_000_000;

        public HashsetLargeCacheBenchmarks()
        {
            _cache = BuildCache();
        }

        [Benchmark]
        public void ReadOperations_SmallDataSet_LargeReadCount()
        {
            var random = new Random();

            for (var i = 0; i <= ReadCount; i++)
            {
                var index = random.Next(1, _searchListCount);

                var searchTerm = _searchList[index];

                _cache.Contains(searchTerm);
            }
        }

        private HashSet<PasswordBlacklist> BuildCache()
        {          
            var passwordEntries = File.ReadAllLines("Cache\\Mocks\\passwordblacklist.txt");

            var passwordBlackist = new List<PasswordBlacklist>();
            var result = new HashSet<PasswordBlacklist>();

            foreach (var entry in passwordEntries)
            {
                var passwordBlackistEntry = new PasswordBlacklist { Value = entry };

                result.Add(passwordBlackistEntry);
                passwordBlackist.Add(passwordBlackistEntry);
            }

            var random = new Random();

            _searchList = new List<PasswordBlacklist>();

            for (var i = 0; i <= _searchListCount; i++)
            {
                var index = random.Next(1, _searchListCount);

                var entry = passwordBlackist[index];

                _searchList.Add(entry);
            }

            return result;
        }
    }
}
