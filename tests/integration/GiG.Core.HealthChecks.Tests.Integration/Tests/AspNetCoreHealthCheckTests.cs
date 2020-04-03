using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class AspNetCoreHealthCheckTests
    {
        private readonly HttpClient _httpClient;
        private readonly HealthCheckOptions _healthCheckOptions;

        public AspNetCoreHealthCheckTests()
        {
            _healthCheckOptions = new HealthCheckOptions();

            _httpClient = new TestServer(new WebHostBuilder()
                    .UseStartup<AspNetCoreMockStartup>()
                    .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")))
                .CreateClient();
        }

        [Fact]
        public async Task HealthChecks_CombinedUrlEndpoint_ReturnsOkStatusCode()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthCheckOptions.CombinedUrl);
            
            // Act
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthChecks_ReadyUrlEndpoint_ReturnsOkStatusCode()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthCheckOptions.ReadyUrl);
            
            // Act
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthChecks_LiveUrlEndpoint_ReturnsOkStatusCode()
        {
            // Arrange
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthCheckOptions.LiveUrl);

            // Act
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
