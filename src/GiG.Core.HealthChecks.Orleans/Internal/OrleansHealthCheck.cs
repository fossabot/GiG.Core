using Microsoft.Extensions.Diagnostics.HealthChecks;
using Orleans;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Orleans.Internal
{
    internal class OrleansHealthCheck : IHealthCheck
    {
        private readonly IClusterClient _clusterClient;

        public OrleansHealthCheck(IClusterClient clusterClient)
        {
            _clusterClient = clusterClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_clusterClient.IsInitialized == false)
            {
                return HealthCheckResult.Healthy("Initializing");
            }

            try
            {
                return await _clusterClient.GetGrain<IHealthCheckGrain>(Guid.Empty).IsHealthyAsync()
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("HealthCheckGrain failed.", ex);
            }
        }
    }
}
