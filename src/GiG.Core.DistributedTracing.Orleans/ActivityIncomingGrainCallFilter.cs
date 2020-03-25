using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;
using OrleansDistributedTracingConstants = GiG.Core.DistributedTracing.Orleans.Internal.Constants;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Grain Call Filter to read Activity Information from incoming calls.
    /// </summary>
    public class ActivityIncomingGrainCallFilter : IIncomingGrainCallFilter
    {
        /// <summary>
        /// Invoke Grain call context.
        /// </summary>
        /// <param name="context">The <see cref="IOutgoingGrainCallContext"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            var traceId = RequestContext.Get(Constants.ActivityHeader) as string;

            var activity = new System.Diagnostics.Activity(OrleansDistributedTracingConstants.IncomingGrainFilterActivityName);
            if (!string.IsNullOrWhiteSpace(traceId))
            {
                activity.SetParentId(traceId);
            }
            activity.Start();

            await context.Invoke();

            activity.Stop();
        }
    }
}