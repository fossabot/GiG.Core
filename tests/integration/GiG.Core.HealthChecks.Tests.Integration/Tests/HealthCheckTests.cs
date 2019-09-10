using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    public class HealthCheckTests
    {
        private readonly HealthChecksOptions _healthChecksOptions;

        public HealthCheckTests()
        {
            _healthChecksOptions = new HealthChecksOptions();
        }

        [Fact]
        public async Task RespondWithHealthyStatusOnCombinedHealthCheck()
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
        public async Task RespondWithHealthyStatusOnLiveHealthCheck()
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
        public async Task RespondWithHealthyStatusOnReadyHealthCheck()
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
        public async Task RespondWithHealthyStatusOnCombinedHealthCheckWithCustomUrl()
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
        public async Task RespondWithHealthyStatusOnLiveHealthCheckWithCustomUrl()
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
        public async Task RespondWithHealthyStatusOnReadyHealthCheckWithCustomUrl()
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
        public async Task RespondWithUnHealthyStatusOnReadyHealthCheck()
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
        public async Task RespondWithUnHealthyStatusOnLiveHealthCheck()
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
        public async Task RespondWithLiveCheckUnHealthyStatusOnCombinedHealthCheck()
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
        public async Task RespondWithReadyCheckUnHealthyStatusOnCombinedHealthCheck()
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