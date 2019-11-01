using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface IClusterClientFactoryTestGrain : IGrainWithStringKey
    {
        Task<string> GetSiloNameAsync();
    }
}