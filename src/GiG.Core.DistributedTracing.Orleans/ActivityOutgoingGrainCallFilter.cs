using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Grain Call Filter to add Activity information in outgoing calls.
    /// </summary>
    public class ActivityOutgoingGrainCallFilter : IOutgoingGrainCallFilter
    {
        /// <summary>
        /// Invoke Grain call context.
        /// </summary>
        /// <param name="context">The <see cref="IOutgoingGrainCallContext"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public Task Invoke(IOutgoingGrainCallContext context)
        {
            if (RequestContext.Get(Constants.ActivityHeader) != null || Activity.Current == null) return context.Invoke();
            
            RequestContext.Set(Constants.ActivityHeader, Activity.Current.Id);
            RequestContext.Set(Constants.BaggageHeader, Activity.Current.Baggage);

            return context.Invoke();
        }
    }
}