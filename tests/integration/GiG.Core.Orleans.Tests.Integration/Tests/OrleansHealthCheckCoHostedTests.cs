using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansHealthCheckCoHostedTests : AbstractHealthCheckTests, IAsyncLifetime
    {
        private readonly  HealthCheckCoHostedLifetime _healthCheckLifetime;

        public OrleansHealthCheckCoHostedTests()
        {
            _healthCheckLifetime = new HealthCheckCoHostedLifetime();
        }

        public async Task InitializeAsync()
        {
            await _healthCheckLifetime.InitializeAsync();

            HttpClientFactory = _healthCheckLifetime.HttpClientFactory;
            HealthCheckOptions = new HealthCheckOptions();
            Port = HealthCheckCoHostedLifetime.Port;
        }

        public async Task DisposeAsync()
        {
            await _healthCheckLifetime.DisposeAsync();
        }
    }
}