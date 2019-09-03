using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GiG.Core.Extensions.HealthCheck.Tests.Integration
{
    public class HealthCheckTests
    {
        private readonly TestServer _testServer;

        public HealthCheckTests()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>());
        }


    }

    public class MockStartup
    {
        [Fact]
        public async Task RespondWithHealthyStatusOnLiveHealthCheck()
        {

        }

        [Fact]
        public async Task RespondWithHealthyStatusOnReadyHealthCheck()
        {

        }
    }
}
