using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface ITestGrain : IGrainWithStringKey
    {
        Task SetValueAsync(int value);
        
        Task<int> GetValueAsync();
    }
}