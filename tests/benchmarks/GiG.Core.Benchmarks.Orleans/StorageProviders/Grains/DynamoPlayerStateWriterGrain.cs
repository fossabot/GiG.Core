using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans.Runtime;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public class DynamoPlayerStateWriterGrain : BasePlayerStateWriteGrain, IDynamoPlayerStateWriterGrain
    {
        public DynamoPlayerStateWriterGrain(
            [PersistentState(StorageProvidersConstants.PlayerState, StorageProvidersConstants.DynamoDb)] IPersistentState<PlayerDetailsState> playerState) 
            : base(playerState)
        {
        }
    }
}