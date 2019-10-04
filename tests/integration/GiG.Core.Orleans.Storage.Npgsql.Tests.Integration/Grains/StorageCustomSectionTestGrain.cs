using GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Contracts;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Grains
{
    [StorageProvider(ProviderName = "testDB2")]
    public class StorageCustomSectionTestGrain : Grain<StorageState>, IStorageCustomSectionTestGrain
    {
        public async Task<string> StoreAndReturnValue(string value)
        {
            State.Value = value;

            await base.WriteStateAsync();

            return State.Value;
        }
    }
}
