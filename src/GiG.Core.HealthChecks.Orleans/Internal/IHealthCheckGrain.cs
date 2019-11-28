using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Orleans.Internal
{
    interface IHealthCheckGrain : IGrainWithGuidKey
    {
        Task<bool> IsHealthy();
    }
}
