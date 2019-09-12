using System.Threading.Tasks;
using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;

namespace GiG.Core.DistributedTracing.Orleans
{
    public class CorrelationGrainCallFilter : IOutgoingGrainCallFilter
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

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