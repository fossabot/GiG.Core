using System;
using GiG.Core.DistributedTracing.Abstractions;
using Orleans.Runtime;

namespace GiG.Core.DistributedTracing.Orleans
{
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        public Guid Value => 
            Guid.TryParse(RequestContext.Get(Constants.Header)?.ToString(), out var correlationId)
                ? correlationId
                : Guid.Empty;
    }
}