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
        /// Correlation Id value represented by the Orleans ActivityId inside RequestContext or Empty Guid if not present.
        /// </summary>
        public Guid Value
        {
            get
            {
                if (RequestContext.ActivityId == Guid.Empty)
                {
                    RequestContext.ActivityId = Guid.NewGuid();
                }
                
                RequestContext.PropagateActivityId = true;
                
                return RequestContext.ActivityId;
            }
        }
    }
}