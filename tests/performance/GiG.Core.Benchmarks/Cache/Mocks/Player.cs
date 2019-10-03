using MasterMemory;
using MessagePack;

namespace GiG.Core.Benchmarks.Cache.Mocks
{
    [MemoryTable("player"), MessagePackObject(true)]
    public class Player
    {
        [PrimaryKey]
        public int PlayerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}