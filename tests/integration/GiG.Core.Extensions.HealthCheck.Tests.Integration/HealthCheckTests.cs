using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using System.Threading.Tasks;
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

        [Fact]
        public async Task RespondWithHealthyStatusOnLiveHealthCheck()
        {
            var client = _testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/health/live");
            using var response = await client.SendAsync(request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RespondWithHealthyStatusOnReadyHealthCheck()
        {
            var client = _testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, "/health/ready");
            using var response = await client.SendAsync(request);

            Assert.NotNull(response);
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        }
    }
}
