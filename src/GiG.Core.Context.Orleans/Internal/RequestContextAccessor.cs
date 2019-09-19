using GiG.Core.Context.Abstractions;
using Orleans.Runtime;
using System.Net;

namespace GiG.Core.Context.Orleans.Internal
{
    internal class RequestContextAccessor : IRequestContextAccessor
    {
        public IPAddress IPAddress => IPAddress.TryParse(RequestContext.Get(Constants.OrleansIPAddressContextKey)?.ToString(), 
            out var ipAddress) ? ipAddress : null;
    }
}