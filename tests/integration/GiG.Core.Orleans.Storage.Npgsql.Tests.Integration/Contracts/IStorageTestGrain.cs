using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Storage.Npgsql.Tests.Integration.Contracts
{
    public interface IStorageTestGrain : IGrainWithStringKey
    {
        Task<string> SetAndReturnValue(string value);
        Task<string> StoreAndReturnValue(string value);
    }
}
