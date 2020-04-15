using GiG.Core.HealthChecks.Abstractions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Integration.Helpers;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using GiG.Core.Logging.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HealthCheckTests
    {
        [Fact]
        public async Task CombinedHealthCheck_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
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
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.LiveUrl);

            // Act
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
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.ReadyUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_WithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task LiveHealthCheck_WithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.LiveUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_WithCustomUrl_ReturnsHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartupWithCustomConfiguration>()
                .ConfigureServices(x => x.AddHealthChecks()));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithCustomConfiguration.ReadyUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheckOfTypeT_ReturnsUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddHealthChecks()
                    .AddReadyCheck<UnHealthyCheck>(nameof(UnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.ReadyUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_ReturnsUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddHealthChecks()
                    .AddReadyCheck<UnHealthyCheck>(nameof(UnHealthyCheck), HealthStatus.Unhealthy)));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.ReadyUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_WithLogging_ReturnsUnHealthyStatus()
        {
            // Act
            var logEvent = await ReadyHealthCheckWithLoggingAsync<UnHealthyCheck>();

            // Assert
            Assert.NotNull(logEvent);
            Assert.Equal("Health check \"UnHealthyCheck\" was Unhealthy threw an exception null", logEvent.MessageTemplate.Render(logEvent.Properties));
        }
        
        [Fact]
        public async Task ReadyHealthCheck_WithLoggingAndException_ReturnsUnHealthyStatus()
        {
            // Act
            var logEvent = await ReadyHealthCheckWithLoggingAsync<UnHealthyCheckWithException>();

            // Assert
            Assert.NotNull(logEvent);
            Assert.NotNull(logEvent.Exception);
            Assert.Equal("Health check \"UnHealthyCheckWithException\" was Unhealthy threw an exception \"Test Exception\"", logEvent.MessageTemplate.Render(logEvent.Properties));
        }

        [Fact]
        public async Task LiveHealthCheckOfTypeT_ReturnsUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddHealthChecks()
                    .AddLiveCheck<UnHealthyCheck>(nameof(UnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.LiveUrl);

            // Act
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
                    .AddHealthChecks()
                    .AddLiveCheck<UnHealthyCheck>(nameof(UnHealthyCheck), HealthStatus.Unhealthy)));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.LiveUrl);

            // Act
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
                    .AddHealthChecks()
                    .AddLiveCheck<UnHealthyCheck>("UnHealthyCheckWithName")));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
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
                    .AddHealthChecks()
                    .AddReadyCheck<UnHealthyCheck>(nameof(UnHealthyCheck))));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_WithCachedHealthCheckTyped_ReturnsReadyCheckUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddCachedCheck<UnHealthyCheck>(nameof(UnHealthyCheck), tags: new[] {Constants.ReadyTag})));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);
            using var request2 =
                new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);
            using var response2 = await client.SendAsync(request2);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response2);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response2.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_WithCachedHealthCheckInstance_ReturnsReadyCheckUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddCachedCheck(nameof(UnHealthyCheck), new UnHealthyCheck(), tags: new[] {Constants.ReadyTag})));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);
            using var request2 =
                new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);
            using var response2 = await client.SendAsync(request2);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response2);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response2.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_WithCachedHealthCheckFactory_ReturnsReadyCheckUnHealthyStatus()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                .ConfigureServices(x => x
                    .AddCachedHealthChecks()
                    .AddCachedCheck(nameof(UnHealthyCheck), _ => new UnHealthyCheck(),
                        tags: new[] {Constants.ReadyTag})));

            var client = testServer.CreateClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);
            using var request2 =
                new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.CombinedUrl);

            // Act
            using var response = await client.SendAsync(request);
            using var response2 = await client.SendAsync(request2);

            // Assert
            Assert.NotNull(response);
            Assert.NotNull(response2);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response2.StatusCode);
        }

        [Fact]
        public async Task CombinedHealthCheck_WithoutCachedHealthChecks_ThrowsApplicationException()
        {
            // Arrange
            var server = new TestServer(new WebHostBuilder()
                .ConfigureServices(x => x.AddHealthChecks().AddCachedCheck(nameof(UnHealthyCheck),
                    _ => new UnHealthyCheck(), tags: new[] {Constants.ReadyTag}))
                .Configure(x => x.UseHealthChecks()));

            using var request = new HttpRequestMessage(HttpMethod.Get, new HealthCheckOptions().CombinedUrl);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => server.CreateClient().SendAsync(request));
        }
        
        private static async Task<LogEvent> ReadyHealthCheckWithLoggingAsync<T>() where T : class, IHealthCheck
        {
            // Arrange
            var semaphore = new SemaphoreSlim(0, 1);
            LogEvent logEvent = null;

            void WriteLog(LogEvent log)
            {
                logEvent = log;
                semaphore.Release();
            }

            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging(x =>
                {
                    x.LoggerConfiguration.Filter.ByIncludingOnly(i =>
                        i.Properties["SourceContext"].ToString() == "\"GiG.Core.HealthChecks\"");
                    x.LoggerConfiguration.WriteTo.Sink(new DelegatingSink(WriteLog));
                })
                .ConfigureWebHostDefaults(x => x.UseTestServer().UseStartup<MockStartupWithDefaultConfiguration>())
                .ConfigureServices(x => x
                    .AddHealthChecks()
                    .AddReadyCheck<T>(typeof(T).Name, HealthStatus.Unhealthy))
                .Build();

            await host.StartAsync();
            var client = host.GetTestClient();

            using var request = new HttpRequestMessage(HttpMethod.Get, MockStartupWithDefaultConfiguration.ReadyUrl);

            // Act
            using var response = await client.SendAsync(request);
            await semaphore.WaitAsync(5000);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);

            return logEvent;
        }
    }
}