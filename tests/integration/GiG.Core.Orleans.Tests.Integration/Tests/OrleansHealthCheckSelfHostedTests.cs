using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Threading.Tasks;
using Xunit;
using OrleansHealthChecks = GiG.Core.HealthChecks.Orleans.Abstractions;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansHealthCheckSelfHostedTests : AbstractHealthCheckTests, IAsyncLifetime
    {
        private readonly HealthCheckSelfHostedLifetime _healthCheckLifetime;

        public OrleansHealthCheckSelfHostedTests()
        {
            _healthCheckLifetime = new HealthCheckSelfHostedLifetime();
        }

        public async Task InitializeAsync()
        {
            await _healthCheckLifetime.InitializeAsync();

            HttpClientFactory = _healthCheckLifetime.HttpClientFactory;
            HealthCheckOptions = new HealthCheckOptions();
            Port = new OrleansHealthChecks.HealthCheckOptions().Port;
        }

        public async Task DisposeAsync()
        {
            await _healthCheckLifetime.DisposeAsync();
        }
    }
}