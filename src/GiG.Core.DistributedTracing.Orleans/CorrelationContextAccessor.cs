using GiG.Core.DistributedTracing.Abstractions;
using Orleans.Runtime;
using System;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Correlation Accessor for Orleans.
    /// </summary>
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        /// <summary>
        /// Correlation Id value or Empty Guid if not present. Internally represented by the Orleans ActivityId inside RequestContext
        /// </summary>
        public Guid Value => RequestContext.ActivityId;               
    }
}