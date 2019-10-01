using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Contracts
{
    public interface IStorageCustomSectionTestGrain : IGrainWithStringKey
    {
        Task<string> StoreAndReturnValue(string value);
    }
}
