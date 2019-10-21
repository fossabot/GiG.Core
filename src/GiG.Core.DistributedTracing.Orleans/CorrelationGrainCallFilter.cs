using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Grain Call Filter to add Correlation ID in outgoing calls.
    /// </summary>
    public class CorrelationGrainCallFilter : IOutgoingGrainCallFilter
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="correlationContextAccessor"></param>
        public CorrelationGrainCallFilter(ICorrelationContextAccessor correlationContextAccessor) => _correlationContextAccessor = correlationContextAccessor;

        /// <summary>
        /// Invoke Grain call context.
        /// </summary>
        /// <param name="context">The <see cref="IOutgoingGrainCallContext"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            RequestContext.Set(Constants.Header, _correlationContextAccessor?.Value);
            await context.Invoke();
        }
    }
}