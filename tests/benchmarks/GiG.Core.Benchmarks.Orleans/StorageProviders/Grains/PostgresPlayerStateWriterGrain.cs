using GiG.Core.Benchmarks.Orleans.StorageProviders.Contracts;
using Orleans.Runtime;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public class PostgresPlayerStateWriterGrain : BasePlayerStateWriteGrain, IPostgresPlayerStateWriterGrain
    {
        public PostgresPlayerStateWriterGrain(
            [PersistentState(StorageProvidersConstants.PlayerState, StorageProvidersConstants.Postgres)] IPersistentState<PlayerDetailsState> playerState) 
            : base(playerState)
        {
        }
    }
}