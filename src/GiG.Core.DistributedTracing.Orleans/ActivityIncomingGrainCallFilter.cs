using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Collections.Generic;
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
            var activity = new System.Diagnostics.Activity(OrleansDistributedTracingConstants.IncomingGrainFilterActivityName);

            if (RequestContext.Get(Constants.ActivityHeader) is string traceId)
            {
                activity.SetParentId(traceId);
            }

            if (RequestContext.Get(Constants.BaggageHeader) is IEnumerable<KeyValuePair<string, string>> baggage)
            {
                foreach (var item in baggage)
                {
                    activity.AddBaggage(item.Key, item.Value);
                }
            }

            activity.Start();

            await context.Invoke();

            activity.Stop();
        }
    }
}