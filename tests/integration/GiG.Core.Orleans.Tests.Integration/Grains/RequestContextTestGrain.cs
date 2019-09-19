using GiG.Core.Context.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using Orleans;
using System.Net;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class RequestContextTestGrain : Grain, IRequestContextTestGrain
    {
        private readonly IRequestContextAccessor _requestContextAccessor;

        public RequestContextTestGrain(IRequestContextAccessor requestContextAccessor)
        {
            _requestContextAccessor = requestContextAccessor;
        }

        public Task<IPAddress> GetIPAddressAsync()
        {
            return Task.FromResult(_requestContextAccessor.IPAddress);
        }
    }
}