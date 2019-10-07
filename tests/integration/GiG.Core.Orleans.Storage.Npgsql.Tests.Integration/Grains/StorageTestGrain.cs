using GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Grains
{
    [StorageProvider(ProviderName = "testDB")]
    public class StorageTestGrain : Grain<StorageState>, IStorageTestGrain
    {
        public Task<string> SetAndReturnValue(string value)
        {
            State.Value = value;

            return Task.FromResult(State.Value);
        }

        public async Task<string> StoreAndReturnValue(string value)
        {
            State.Value = value;

            await base.WriteStateAsync();

            return State.Value;
        }
    }
}
