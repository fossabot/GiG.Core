using Orleans;
using System.Net;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public interface IRequestContextTestGrain : IGrainWithStringKey
    {
        Task<IPAddress> GetIPAddressAsync();
    }
}