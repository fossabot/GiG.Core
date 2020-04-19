using GiG.Core.Metrics.Abstractions;
using GiG.Core.Metrics.Prometheus.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Metrics.Prometheus.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ApplicationMetricsTests
    {
        private readonly HttpClient _httpClient;

        public ApplicationMetricsTests()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<MockStartup>();
                    webHost.ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json"));
                });

            _httpClient = hostBuilder.Start().GetTestClient();
        }
       
        [Fact]
        public async Task ApplicationMetricEndpoint_ReturnsOk()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, new ApplicationMetricsOptions().Url);
            
            // Act
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}