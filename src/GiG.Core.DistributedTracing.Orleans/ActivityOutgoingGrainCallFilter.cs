using GiG.Core.DistributedTracing.Abstractions;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.Orleans
{
    /// <summary>
    /// Grain Call Filter to add Activity information in outgoing calls.
    /// </summary>
    public class ActivityOutgoingGrainCallFilter : IOutgoingGrainCallFilter
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="activityContextAccessor">The <see cref="IActivityContextAccessor"/>.</param>
        public ActivityOutgoingGrainCallFilter(IActivityContextAccessor activityContextAccessor) => _activityContextAccessor = activityContextAccessor;

        /// <summary>
        /// Invoke Grain call context.
        /// </summary>
        /// <param name="context">The <see cref="IOutgoingGrainCallContext"/>.</param>
        /// <returns>A <see cref="Task"/>.</returns>
        public async Task Invoke(IOutgoingGrainCallContext context)
        {
            RequestContext.Set(Constants.ActivityHeader, _activityContextAccessor.CorrelationId);
            await context.Invoke();
        }
    }
}