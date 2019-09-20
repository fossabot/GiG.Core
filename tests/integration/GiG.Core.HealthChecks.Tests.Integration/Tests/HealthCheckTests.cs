using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HealthCheckTests
    {
        private readonly HealthChecksOptions _healthChecksOptions;

        public HealthCheckTests()
        {
            _healthChecksOptions = new HealthChecksOptions();
        }

        [Fact]
        public async Task CombinedHealthCheck_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.CombinedUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LiveHealthCheck_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.LiveUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.ReadyUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheckWithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.CombinedUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LiveHealthCheckWithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.LiveUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheckWithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddCachedHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.ReadyUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_ReturnsUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddReadyCheck<CachedUnHealthyCheck>(nameof(CachedUnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.ReadyUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task LiveHealthCheck_ReturnsUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddLiveCheck<CachedUnHealthyCheck>(nameof(CachedUnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.LiveUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_ReturnsLiveCheckUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddLiveCheck<CachedUnHealthyCheck>(nameof(CachedUnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.CombinedUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_ReturnsReadyCheckUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddReadyCheck<CachedUnHealthyCheck>(nameof(CachedUnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, _healthChecksOptions.CombinedUrl);
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }
    }
}