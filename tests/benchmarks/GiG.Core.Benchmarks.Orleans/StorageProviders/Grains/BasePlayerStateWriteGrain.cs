using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Orleans.StorageProviders.Grains
{
    public abstract class BasePlayerStateWriteGrain : Grain
    {
        private readonly IPersistentState<PlayerDetailsState> _playerState;

        protected BasePlayerStateWriteGrain(IPersistentState<PlayerDetailsState> playerState)
        {
            _playerState = playerState;
        }
        
        public async Task WritePlayerDetailAsync(string firstName, string lastName)
        {
            _playerState.State.FirstName = firstName;
            _playerState.State.LastName = lastName;

            await _playerState.WriteStateAsync();
        }
        
        public async Task<PlayerDetailsState> ReadPlayerDetailAsync()
        {
            //this is to refresh the state from the underlying persistence storage.
            await _playerState.ReadStateAsync();
            
            return _playerState.State;
        }
    }
}