using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans.Runtime;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public class MongoPlayerStateWriterGrain : BasePlayerStateWriteGrain, IMongoPlayerStateWriterGrain
    {
        public MongoPlayerStateWriterGrain(
            [PersistentState(StorageProvidersConstants.PlayerState, StorageProvidersConstants.MongoDb)]IPersistentState<PlayerDetailsState> playerState) 
            : base(playerState)
        {
        }
    }
}