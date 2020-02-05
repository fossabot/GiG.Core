using Orleans;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Orleans.Internal
{
    internal interface IHealthCheckGrain : IGrainWithGuidKey
    {
        Task<bool> IsHealthyAsync();
    }
}
