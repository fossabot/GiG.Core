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
        private readonly HealthChecksOptions _healthChecksOptions;

        public AspNetCoreHealthCheckTests()
        {
            _healthChecksOptions = new HealthChecksOptions();

            _httpClient = new TestServer(new WebHostBuilder()
                    .UseStartup<AspNetCoreMockStartup>()
                    .ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json")))
                .CreateClient();
        }

        [Fact]
        public async Task HealthChecks_CombinedUrlEndpoint_ReturnsOkStatusCode()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.CombinedUrl);
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthChecks_ReadyUrlEndpoint_ReturnsOkStatusCode()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.ReadyUrl);
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task HealthChecks_LiveUrlEndpoint_ReturnsOkStatusCode()
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.LiveUrl);
            using var response = await _httpClient.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
