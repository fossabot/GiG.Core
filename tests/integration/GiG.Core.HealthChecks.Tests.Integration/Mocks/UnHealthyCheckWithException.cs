using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal class UnHealthyCheckWithException : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: new Exception("Test Exception")));
        }
    }
}