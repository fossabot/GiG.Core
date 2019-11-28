using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Orleans.Internal
{
    [StatelessWorker(1)]
    internal class HealthCheckGrain : Grain, IHealthCheckGrain
    {
        public Task<bool> IsHealthy()
        {
            return Task.FromResult(true);
        }
    }
}
