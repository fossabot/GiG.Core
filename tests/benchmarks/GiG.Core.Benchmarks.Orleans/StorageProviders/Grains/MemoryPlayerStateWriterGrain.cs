using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans.Runtime;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public class MemoryPlayerStateWriterGrain : BasePlayerStateWriteGrain, IMemoryPlayerStateWritesGrain
    {
        public MemoryPlayerStateWriterGrain(
            [PersistentState(StorageProvidersConstants.PlayerState, StorageProvidersConstants.InMemory)]IPersistentState<PlayerDetailsState> playerState) 
            : base(playerState)
        {
        }
    }
}