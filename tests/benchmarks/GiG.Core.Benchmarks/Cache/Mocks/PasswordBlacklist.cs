using MasterMemory;
using MessagePack;

namespace GiG.Core.Benchmarks.Cache.Mocks
{
    // ReSharper disable once StringLiteralTypo
    [MemoryTable("passwordblacklist"), MessagePackObject(true)]
    public class PasswordBlacklist
    {
        [PrimaryKey]
        public string Value { get; set; }
    }
}