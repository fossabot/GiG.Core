using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansHealthCheckCoHostedTests : AbstractHealthCheckTests ,IClassFixture<HealthCheckCoHostedFixture>
    {
        public OrleansHealthCheckCoHostedTests(HealthCheckCoHostedFixture healthCheckFixture)
        {
            HttpClientFactory = healthCheckFixture.HttpClientFactory;
            HealthChecksOptions = new HealthChecksOptions();
            Port = healthCheckFixture.Port;
        }
    }
}