using System.Threading.Tasks;
using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Gain Filter to add correlation id in outgoing calls.
    /// </summary>
    public class CorrelationGrainCallFilter : IOutgoingGrainCallFilter
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="correlationContextAccessor"></param>
        public CorrelationGrainCallFilter(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            RequestContext.Set(Constants.Header, _correlationContextAccessor?.Value);
            await context.Invoke();
        }
    }
}