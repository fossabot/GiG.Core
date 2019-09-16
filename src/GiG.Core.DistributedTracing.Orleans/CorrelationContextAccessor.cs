using System;
using GiG.Core.DistributedTracing.Abstractions;
using Orleans.Runtime;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Correlation Accessor for Orleans.
    /// </summary>
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        /// <summary>
        /// Correlation Id value or Empty Guid if not present.
        /// </summary>
        public Guid Value => 
            Guid.TryParse(RequestContext.Get(Constants.Header)?.ToString(), out var correlationId)
                ? correlationId
                : Guid.Empty;
    }
}