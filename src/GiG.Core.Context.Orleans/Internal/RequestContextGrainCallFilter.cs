using GiG.Core.Context.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace GiG.Core.Context.Orleans.Internal
{
    internal class RequestContextGrainCallFilter : IOutgoingGrainCallFilter
    {
        private readonly IRequestContextAccessor _requestContextAccessor;

        public RequestContextGrainCallFilter(IRequestContextAccessor requestContextAccessor)
        {
            _requestContextAccessor = requestContextAccessor;
        }

        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            RequestContext.Set(Constants.OrleansIPAddressContextKey, 
                _requestContextAccessor?.IPAddress?.ToString());
            await context.Invoke();
        }
    }
}
