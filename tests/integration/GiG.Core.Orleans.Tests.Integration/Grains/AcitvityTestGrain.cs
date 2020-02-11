using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class AcitvityTestGrain : Grain, IActivityTestGrain
    {
        private readonly IActivityContextAccessor _activityContextAccessor;

        public AcitvityTestGrain(IActivityContextAccessor activityContextAccessor)
        {
            _activityContextAccessor = activityContextAccessor;
        }

        public Task<ActivityResponse> GetActivityAsync()
        {
            var activity = new ActivityResponse
            {
                TraceId = _activityContextAccessor.TraceId,
                ParentId = _activityContextAccessor.ParentId
            };
            return Task.FromResult(activity);
        }
    }
}