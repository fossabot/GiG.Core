using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using OrleansHealthChecks = GiG.Core.HealthChecks.Orleans.Abstractions;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansHealthCheckSelfHostedTests : AbstractHealthCheckTests, IClassFixture<HealthCheckSelfHostedFixture>
    {
        public OrleansHealthCheckSelfHostedTests(HealthCheckSelfHostedFixture healthCheckFixture)
        {
            HttpClientFactory = healthCheckFixture.HttpClientFactory;
            HealthChecksOptions = new HealthChecksOptions();
            Port = new OrleansHealthChecks.HealthChecksOptions().Port;
        }
    }
}