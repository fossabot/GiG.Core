using GiG.Core.ApplicationMetrics.Abstractions;
using GiG.Core.ApplicationMetrics.Prometheus.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.ApplicationMetrics.Prometheus.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ApplicationMetricsTests
    {
       
        [Fact]
        public async Task ApplicationMetricEndpoint_ReturnsOk()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());

            using var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, new ApplicationMetricsOptions().Url);
            
            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}