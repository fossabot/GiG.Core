using MasterMemory;
using MessagePack;

namespace GiG.Core.Benchmarks.Cache.Mocks
{
    [MemoryTable("passwordblacklist"), MessagePackObject(true)]
    public class PasswordBlacklist
    {
        [PrimaryKey]
        public string Value { get; set; }
    }
}