using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans.Runtime;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public class RedisPlayerStateWriterGrain : BasePlayerStateWriteGrain, IRedisPlayerStateWriterGrain
    {
        public RedisPlayerStateWriterGrain(
            [PersistentState(StorageProvidersConstants.PlayerState, StorageProvidersConstants.Redis)] IPersistentState<PlayerDetailsState> playerState) 
            : base(playerState)
        {
        }
    }
}