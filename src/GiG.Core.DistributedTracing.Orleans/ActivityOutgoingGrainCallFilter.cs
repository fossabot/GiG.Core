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
        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            if (Activity.Current != null)
            {
                RequestContext.Set("Trace-Parent", Activity.Current.Id);
                RequestContext.Set("Correlation-Context", Activity.Current.Baggage);
            }
                
            await context.Invoke();
        }
    }
}